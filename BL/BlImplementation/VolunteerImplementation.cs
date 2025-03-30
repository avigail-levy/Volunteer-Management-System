using BlApi;
using Helpers;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddVolunteer(BO.Volunteer newBoVolunteer)
    {
        DO.Volunteer doVolunteer = VolunteerManager.CreateDoVolunteer(newBoVolunteer);
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={newBoVolunteer.Id} already exists", ex);
        }
    }

    public void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(volunteer.Id) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteer.Id} does Not exist");//מיותר?כי הרי כשמזמנים את הפעולה הזאת שולחים מתנדב מוכן וכבר מה בודקים אם קיים או לא
        DO.Volunteer requester = _dal.Volunteer.Read(idRequester);
        if (requester.Role != DO.Role.Manager && idRequester != volunteer.Id)
            throw new BO.BlUnauthorizedException("Only a manager can update the volunteer's role");
        DO.Volunteer updatedDoVolunteer = VolunteerManager.CreateDoVolunteer(volunteer,
            requester.Role == DO.Role.Manager ? (DO.Role)volunteer.Role : null);
        try
        {
            _dal.Volunteer.Update(updatedDoVolunteer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantUpdateException($"volunteer with ID={volunteer.Id} is not exists", ex);
        }
    }
    public void DeleteVolunteer(int idVolunteer)
    {
        var assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == idVolunteer);
        if (assignments.Any(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled || a.TypeOfTreatmentTermination == null))
            throw new BO.BlCantDeleteException("It is not possible to delete a volunteer handling calls or handled in past.");
        try
        {
            _dal.Volunteer.Delete(idVolunteer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantDeleteException("It is not possible to delete the volunteer", ex);
        }
    }
    /// <summary>
    /// Sorts and filters the collection according to the request received.
    /// </summary>
    /// <param name="active">A Boolean value that will filter the list by active and inactive volunteers.</param>
    /// <param name="sortByAttribute">A field in the "Volunteer on List" entity, by which the list is sorted</param>
    /// <returns>Sorted and filtered threshold of logical data entity "Volunteer in list"</returns>
    public IEnumerable<BO.VolunteerInList> GetListVolunteers(bool? active = null, BO.VolunteerInListAttributes? sortByAttribute = null)
    {
        var vols = _dal.Volunteer.ReadAll();
        vols = active != null ?
               from v in vols
               where v.Active == active
               select v
               : vols;
        var propertySort = typeof(DO.Volunteer).GetProperty(sortByAttribute.ToString());

        vols = propertySort != null ?
              from v in vols
              orderby propertySort.GetValue(v, null)
              select v
              :
              from v in vols
              orderby v.Id
              select v;
        return vols.Select(v =>
        {
            var assignVol = _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id);
            DO.Assignment? assignInTreatment = VolunteerManager.GetCallInTreatment(v.Id);
            DO.Call? call = Tools.GetCallByAssignment(assignInTreatment);
            return new BO.VolunteerInList
            {

                Id = v.Id,
                Name = v.Name,
                Active = v.Active,
                TotalCallsHandledByVolunteer = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.Handled, assignVol),
                TotalCallsCanceledByVolunteer = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.SelfCancellation, assignVol)
                + VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.CancelAdministrator, assignVol),
                TotalExpiredCallingsByVolunteer = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.CancellationExpired, assignVol),
                IDCallInHisCare = call?.Id,
                CallType = (BO.CallType?)call?.CallType ?? BO.CallType.None
            };
        });
    }
    /// <summary>
    /// Please refer to the data layer (Read) to obtain details about the volunteer and the read he/she is handling (if any).
    /// </summary>
    /// <param name="idVolunteer">volunteer id</param>
    /// <returns>An object of the logical entity type "Volunteer" (BO.Volunteer)
    ///which includes an object of the logical entity type "Volunteer Care Call"</returns>
    /// <exception cref="BO.BlDoesNotExistException"> the volunteer not exist</exception>
    public BO.Volunteer GetVolunteerDetails(int idVolunteer)
    {
        DO.Volunteer vol = _dal.Volunteer.Read(idVolunteer) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID {idVolunteer} is not found in database.");
        var assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == idVolunteer);
        DO.Assignment? assignInTreatment = VolunteerManager.GetCallInTreatment(idVolunteer);
        var call = Tools.GetCallByAssignment(assignInTreatment);
        var volunteerBO = new BO.Volunteer
        {
            Id = vol.Id,
            Name = vol.Name,
            Phone = vol.Phone,
            Email = vol.Email,
            Password = vol.Password,
            Address = vol.Address,
            Latitude = vol.Latitude,
            Longitude = vol.Longitude,
            Role = (BO.Role)vol.Role,
            Active = vol.Active,
            MaxDistanceForCall = vol.MaxDistanceForCall,
            DistanceType = (BO.DistanceType)vol.DistanceType,
            TotalCallsHandled = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.Handled, assignments),
            TotalCallsCanceled = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.SelfCancellation, assignments),
            TotalCallsChoseHandleHaveExpired = VolunteerManager.CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination.CancellationExpired, assignments),
            CallingVolunteerTherapy = assignInTreatment != null ? new BO.CallInProgress
            {
                Id = assignInTreatment.Id,
                CallId = assignInTreatment.CallId,
                CallType = (BO.CallType)call.CallType,
                CallDescription = call.CallDescription,
                CallAddress = call.CallAddress,
                OpeningTime = call.OpeningTime,
                MaxTimeFinishCall = call.MaxTimeFinishCall,
                EntryTimeForTreatment = assignInTreatment.EntryTimeForTreatment,
                CallingDistanceFromTreatingVolunteer = VolunteerManager.CalcDistance(vol.Address,call.CallAddress),
                StatusCalling = VolunteerManager.GetCallInProgress(call),
            } : null,
        };
        return volunteerBO;
    }
    /// <summary>
    /// login and return the role
    /// </summary>
    /// <param name="username">user name</param>
    /// <returns>the role of volunteer</returns>
    /// <exception cref="BO.BlDoesNotExistException">the volunteer does not exist</exception>
    public BO.Role Login(string username)
    {
        DO.Volunteer vol = _dal.Volunteer.Read(vol => vol.Name == username) ??
        throw new BO.BlDoesNotExistException($"Volunteer with Name ={username} does Not exist");
        return (BO.Role)vol.Role;
    }

}
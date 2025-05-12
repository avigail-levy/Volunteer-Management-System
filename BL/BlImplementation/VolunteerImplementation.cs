using BlApi;
using Helpers;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// From the logical object details, creates a new object of the data entity type DO.Voluteer
    ///Performs an attempt to request the addition of the new volunteer to the data layer(Create)
    /// </summary>
    /// <param name="newBoVolunteer">An object of the logical entity type "volunteer"</param>
    /// <exception cref="BO.BlAlreadyExistsException">There is a volunteer with this id}</exception>
    public void AddVolunteer(BO.Volunteer newBoVolunteer)
    {
        DO.Volunteer doVolunteer = VolunteerManager.CreateDoVolunteer(newBoVolunteer);
        try
        {
            _dal.Volunteer.Create(doVolunteer);
            VolunteerManager.Observers.NotifyListUpdated(); //stage 5

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={newBoVolunteer.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Requests the record from the data layer and checks which fields have changed
    /// From the details of the logical object BO.Volunteer, creates an object of the data entity type DO.Volunteer
    ///Attempts to request an update of the volunteer in the data layer DO.Volunteer
    /// </summary>
    /// <param name="idVolunteer">ID of the person requesting the update</param>
    /// <param name="volunteer">An object of the logical entity type "volunteer" for update</param>
    /// <exception cref="BO.BlDoesNotExistException">The volunteer is not exist</exception>
    /// <exception cref="BO.BlUnauthorizedException"> There is no authorization</exception>
    /// <exception cref="BO.BlCantUpdateException">cant update volunteer details</exception>
    public void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(volunteer.Id) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteer.Id} does Not exist");//מיותר?כי הרי כשמזמנים את הפעולה הזאת שולחים מתנדב מוכן וכבר מה בודקים אם קיים או לא
        DO.Volunteer requester = _dal.Volunteer.Read(idRequester)!;
        if (requester.Role != DO.Role.Manager && idRequester != volunteer.Id)
            throw new BO.BlUnauthorizedException("Only a manager can update the volunteer's role");
        DO.Volunteer updatedDoVolunteer = VolunteerManager.CreateDoVolunteer(volunteer,
            requester.Role == DO.Role.Manager ? (DO.Role)volunteer.Role : null);
        try
        {
            _dal.Volunteer.Update(updatedDoVolunteer);
            VolunteerManager.Observers.NotifyItemUpdated(updatedDoVolunteer.Id); //stage 5
            VolunteerManager.Observers.NotifyListUpdated(); //stage 5
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantUpdateException($"volunteer with ID={volunteer.Id} is not exists", ex);
        }
    }
    /// <summary>
    /// Requesting a request to the data layer to check if the volunteer can be deleted
    ///Attempting to request a deletion of the volunteer from the data layer
    /// </summary>
    /// <param name="idVolunteer">Volunteer ID</param>
    /// <exception cref="BO.BlCantDeleteException"></exception>
    public void DeleteVolunteer(int idVolunteer)
    {
        var assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == idVolunteer);
        if (assignments.Any(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled || a.TypeOfTreatmentTermination == null))
            throw new BO.BlCantDeleteException("It is not possible to delete a volunteer handling calls or handled in past.");
        try
        {
            _dal.Volunteer.Delete(idVolunteer);
            VolunteerManager.Observers.NotifyListUpdated(); //stage 5

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
        var propertySort = sortByAttribute != null ? typeof(DO.Volunteer).GetProperty(sortByAttribute.ToString()!) : null;

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
            DO.Call? call = AssignmentManager.GetCallByAssignment(assignInTreatment);
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
                //CallType = (BO.CallType?)call?.CallType ?? BO.CallType.None
                CallType = call is not null? (BO.CallType)call.CallType : BO.CallType.None
    
   
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
        var call = AssignmentManager.GetCallByAssignment(assignInTreatment);
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
                CallType = (BO.CallType)call!.CallType,
                CallDescription = call.CallDescription,
                CallAddress = call.CallAddress,
                OpeningTime = call.OpeningTime,
                MaxTimeFinishCall = call.MaxTimeFinishCall,
                EntryTimeForTreatment = assignInTreatment.EntryTimeForTreatment,
                CallingDistanceFromTreatingVolunteer = VolunteerManager.CalcDistance(vol.Address, call.CallAddress),
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


    /// <summary>
    /// Returns a sorted and filtered collection of entities "call  in a list"
    /// </summary>
    /// <param name="filterByAttribute">A field in the "callInList" entity by which the list will be filtered</param>
    /// <param name="filterValue">Value to filter</param>
    /// <param name="sortByAttribute">a field in the "List Read" entity, by which the list is sorted</param>
    /// <returns>call list</returns>
    public IEnumerable<BO.VolunteerInList> GetVolunteersList(BO.VolunteerInListAttributes? filterByAttribute = null, object? filterValue = null, BO.VolunteerInListAttributes? sortByAttribute = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll();

        var propertyFilter = filterByAttribute != null ? typeof(DO.Volunteer).GetProperty(filterByAttribute.ToString()!) : null;

        volunteers = propertyFilter != null ?
                from v in volunteers
                where propertyFilter.GetValue(v, null) == filterValue
                select v
                :
                from item in volunteers
                select item;


        var propertySort = sortByAttribute != null ? typeof(DO.Call).GetProperty(sortByAttribute.ToString()!) : null;

        volunteers = sortByAttribute != null ?
                from v in volunteers
                orderby propertySort!.GetValue(v, null)
                select v
                :
                from v in volunteers
                orderby v.Id
                select v;

        return volunteers.Select(v =>
        {

            var assignVol = _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id);
            DO.Assignment? assignInTreatment = VolunteerManager.GetCallInTreatment(v.Id);
            DO.Call? call = AssignmentManager.GetCallByAssignment(assignInTreatment);
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
                CallType = (BO.CallType?)(call?.CallType) ?? BO.CallType.None
            };
        });
    }



    #region Stage 5
    public void AddObserver(Action listObserver) =>
    VolunteerManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    VolunteerManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    VolunteerManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    VolunteerManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5


}
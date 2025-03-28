using BlApi;
using Helpers;
namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(BO.Volunteer newBoVolunteer)
    {
        DO.Volunteer doVolunteer =
         new(newBoVolunteer.Id,
             newBoVolunteer.Name,
             newBoVolunteer.Phone,
             newBoVolunteer.Email,
             (DO.Role)newBoVolunteer.Role,
             newBoVolunteer.Active,
             (DO.DistanceType)newBoVolunteer.DistanceType,
             newBoVolunteer.Latitude,
             newBoVolunteer.Longitude,
             newBoVolunteer.Password,
             newBoVolunteer.Address,
             newBoVolunteer.MaxDistanceForCall
             );
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={newBoVolunteer.Id} already exists", ex);
        }
    }

    public void DeleteVolunteer(int idVolunteer)
    {//מתנדב שהוקצאה לו קריאה אבל הוא עוד לא התחיל לטפל בה גם מפריע למחיקת מתנדב
        try
        {
            var assignments = _dal.Assignment.ReadAll()
                              .Where(a => a.VolunteerId == idVolunteer)
                              .Select(a => a.VolunteerId);
            if (assignments.Count() != 0)
                throw new BO.BlCantDeleteException("It is not possible to delete a volunteer handling calls.");
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
    public IEnumerable<BO.VolunteerInList> GetListVolunteers(bool? active=null, BO.VolunteerInListAttributes? sortByAttribute=null)
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
            DO.Call call = Tools.GetCallByAssignment(assignInTreatment);
            return new BO.VolunteerInList
            {

                Id = v.Id,
                Name = v.Name,
                Active = v.Active,
                TotalCallsHandledByVolunteer = (from a in assignVol
                                                where a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled
                                                select a).Count(),
                TotalCallsCanceledByVolunteer = (from a in assignVol
                                                 where a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.SelfCancellation
                                                 select a).Count(),
                TotalExpiredCallingsByVolunteer = (from a in assignVol
                                                   where a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired
                                                   select a).Count(),

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
            TotalCallsHandled = _dal.Assignment.ReadAll(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled)
                .Count(),
            TotalCallsCanceled = _dal.Assignment.ReadAll(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.SelfCancellation)
                .Count(),
            TotalCallsChoseHandleHaveExpired = _dal.Assignment.ReadAll(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired)
                .Count(),

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
                CallingDistanceFromTreatingVolunteer = Tools.CalcDistance(call.Latitude, call.Longitude, vol.Latitude, vol.Longitude),
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
    public void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(volunteer.Id) ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteer.Id} does Not exist");
        try
        {
            DO.Volunteer requester = _dal.Volunteer.Read(idRequester);//לבדוק אם מי שמבקש הוא מנהל או שלפחות זה באמת המתנדב בעצמו
            if (requester.Role != DO.Role.Manager && idRequester != volunteer.Id)
                throw new BO.BlUnauthorizedException("Only a managar can update the volunteer's Position");
            {
                //בדיקות תקינות לעדכון
                //יש לבקש את הרשומה משכבת הנתונים ולבדוק אילו שדות השתנו מה הכוונה?

                DO.Volunteer updatedDoVolunteer = new(
                volunteer.Id,
                volunteer.Name,
                volunteer.Phone,
                volunteer.Email,
                requester.Role == DO.Role.Manager ? (DO.Role)volunteer.Role : (DO.Role)_dal.Volunteer.Read(volunteer.Id).Role, // רק מנהל יכול לשנות תפקיד
                volunteer.Active,
                (DO.DistanceType)volunteer.DistanceType,
                volunteer.Latitude,//לעדכן קווי אורך ורוחב בהתאם לכתובת פונקציית עזר 
                volunteer.Longitude,
                volunteer.Password,
                volunteer.Address,
                volunteer.MaxDistanceForCall
                 );
                _dal.Volunteer.Update(updatedDoVolunteer);
            }
        }
        catch (DO.DalDoesNotExistException e)//לעשות חריגה חדשה  מתאימה
        {
            throw new BO.BlCantUpdateException("Only a manager can update the volunteer's Role", e);
        }
    }
}

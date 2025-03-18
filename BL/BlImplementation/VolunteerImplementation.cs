using BlApi;
using BO;
using Helpers;
using System;

namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(Volunteer newBoVolunteer)
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
            throw new BlAlreadyExistsException($"Volunteer with ID={newBoVolunteer.Id} already exists", ex);
        }
    }

    public void DeleteVolunteer(int idVolunteer)
    {
        try
        {
            var assignments = _dal.Assignment.ReadAll()
                              .Where(a => a.VolunteerId == idVolunteer)
                              .Select(a => a.VolunteerId);
            if (!assignments.Any())
                throw new BlCantDeleteException("It is not possible to delete a volunteer handling calls.");
            _dal.Volunteer.Delete(idVolunteer);
        }
        catch (Exception ex)
        {
            throw new BlCantDeleteException("It is not possible to delete the volunteer", ex);
        }
    }

    public IEnumerable<VolunteerInList> GetListVolunteers(bool? active, VolunteerInListAttributes? filterByAttribute)
    {
        throw new NotImplementedException();
    }

    public Volunteer GetVolunteerDetails(int idVolunteer)
    {
        try
        {
            var vol = _dal.Volunteer.Read(idVolunteer) ?? throw new Exception($"Volunteer with ID {idVolunteer} is not found in database.");

            //var assignment = _dal.Assignment.Read(a => a.VolunteerId == idVolunteer);
            var volunteerBO = new Volunteer
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
                TotalCallsHandled = _dal.Assignment.ReadAll()
                    .Count(a => a.VolunteerId == idVolunteer && a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled),
                TotalCallsCanceled = _dal.Assignment.ReadAll()
                    .Count(a => a.VolunteerId == idVolunteer && a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.SelfCancellation),
                TotalCallsChoseHandleHaveExpired = _dal.Assignment.ReadAll()
                    .Count(a => a.VolunteerId == idVolunteer && a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired),

//                CallingVolunteerTherapy = _dal.Assignment.Read(a => a.VolunteerId == idVolunteer) ?? new CallInProgress
//                {
//                      Id
//                      CallId 
//                      CallType 
//                      CallDescription//תיאור מילולי של הקריאה
//                      CallAddress 
//                      OpeningTime 
//                      MaxTimeFinishCall 
//                      EntryTimeForTreatment //maybeeeee init??
//                      CallingDistanceFromTreatingVolunteer
//    public StatusCalling StatusCalling { get; set; }
//} : null,
            };
            return volunteerBO;

        }
        catch (Exception ex)
        {
            throw new Exception("bla", ex);
        }
    }
    public Role Login(string username)
    {
        
    }
    public void UpdateVolunteerDetails(int idVolunteer, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}

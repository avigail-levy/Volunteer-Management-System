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
            DO.Volunteer vol = _dal.Volunteer.Read(idVolunteer) ?? throw new BlDoesNotExistException($"Volunteer with ID {idVolunteer} is not found in database.");

            var assignment = _dal.Assignment.Read(a => a.VolunteerId == idVolunteer);
            var call = _dal.Call.Read(assignment.CallId);

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

                CallingVolunteerTherapy = assignment!=null ? new CallInProgress
                {
                      //Id= 
                      CallId = assignment.CallId,
                      CallType = (BO.CallType)call.CallType,
                      CallDescription=call.CallDescription,
                      CallAddress = call.CallAddress,
                      OpeningTime = call.OpeningTime,
                      MaxTimeFinishCall =call.MaxTimeFinishCall,
                      EntryTimeForTreatment =assignment.EntryTimeForTreatment, //maybeeeee init??
                      CallingDistanceFromTreatingVolunteer =
                      StatusCalling = 
                } : null,
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
        var vol = _dal.Volunteer.Read(vol => vol.Name == username ) ??
        throw new BO.BlDoesNotExistException($"Volunteer with Name ={username} does Not exist");
        return (BO.Role)vol.Role;
    }
    public void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(volunteer.Id)?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteer.Id} does Not exist");
        
        //tryלכאורה מיותר כי אין בכלל זריקה מהדאל בריד אבל למה???
        //{
        //    doVolunteer = _dal.Volunteer.Read(volunteer.Id);

        //}
        //catch (DO.DalDoesNotExistException e)
        //{
           
        //}
        try
        {
            DO.Volunteer requester = _dal.Volunteer.Read(idRequester);//לבדוק אם מי שמבקש הוא מנהל או שלפחות זה באמת המתנדב בעצמו
            if (requester.Role != DO.Role.Manager && idRequester != volunteer.Id)
                throw new Exception("bbbb");//חריגה מתאימה
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
        catch(BO.BlDoesNotExistException e)//לעשות חריגה חדשה  מתאימה
        {
              throw new BlDoesNotExistException($"you are not a manager or it's not your id{idRequester}",e);
        }
    }
}

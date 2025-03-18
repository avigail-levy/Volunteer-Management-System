using BlApi;
using BO;

using Helpers;
using System;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    
    public void AddVolunteer(Volunteer newBoVolunteer)
    {
        try
        {
            Tools.IntegrityCheck(newBoVolunteer);
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
        throw new NotImplementedException();
    }

    public Role Login(string username)
    {
    }

    public void UpdateVolunteerDetails(int idVolunteer, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
}
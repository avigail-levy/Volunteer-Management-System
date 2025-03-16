using BlApi;
using BO;

namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(Volunteer newBoVolunteer)
    {
        throw new NotImplementedException();
    }

    public void DeleteVolunteer(int idVolunteer)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void UpdateVolunteerDetails(int idVolunteer, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}

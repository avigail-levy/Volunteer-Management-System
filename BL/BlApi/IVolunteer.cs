namespace BlApi
{
    public interface IVolunteer: IObservable
    {
        IEnumerable<BO.VolunteerInList> GetListVolunteers(bool? active , BO.VolunteerInListAttributes? sortByAttribute);// במימוש לכתוב בכותרת =נאל ע"פ הדרישות
        
        BO.Volunteer GetVolunteerDetails (int idVolunteer);
        
        void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer);
        
        void DeleteVolunteer(int idVolunteer);

        void AddVolunteer(BO.Volunteer newBoVolunteer);
        IEnumerable<BO.VolunteerInList> GetVolunteersList(BO.VolunteerInListAttributes? filterByAttribute, object? filterValue, BO.VolunteerInListAttributes? sortByAttribute);//null בכותרת המימוש

    }
}

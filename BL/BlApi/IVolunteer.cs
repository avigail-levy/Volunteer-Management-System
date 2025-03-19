namespace BlApi
{
    public interface IVolunteer
    {
        /// <summary>
        /// Importing and returning the user role
        /// </summary>
        /// <param name="username">user name</param>
        /// <returns>User role</returns>
        BO.Role Login(string username);
        /// <summary>
        /// The method will sort and filter the variables according to the parameters received.
        /// </summary>
        /// <param name="active">A Boolean value that will filter the list by active and inactive volunteers.</param>
        /// <param name="filterByAttribute">ENUM value of a field in the "Volunteer in List" entity, by which the list is sorted</param>
        /// <returns>Returns a sorted and filtered collection of the logical data entity "Volunteer in List"</returns>
        IEnumerable<BO.VolunteerInList> GetListVolunteers(bool? active , BO.VolunteerInListAttributes? filterByAttribute);// במימוש לכתוב בכותרת =נאל ע"פ הדרישות
        /// <summary>
        /// Requests the data layer (Read) to obtain details about the volunteer and the call in his care (if any)
        ///From the details received, creates an object of the logical entity type "Volunteer" 
        ///that includes an object of the logical entity type "Call in Volunteer Care"
        /// </summary>
        /// <param name="idVolunteer">Volunteer ID</param>
        /// <returns>Returns the object it constructed.</returns>
        BO.Volunteer GetVolunteerDetails (int idVolunteer);
        /// <summary>
        /// Requests the record from the data layer and checks which fields have changed
        /// From the details of the logical object BO.Volunteer, creates an object of the data entity type DO.Volunteer
        ///Attempts to request an update of the volunteer in the data layer DO.Volunteer
        /// </summary>
        /// <param name="idVolunteer">ID of the person requesting the update</param>
        /// <param name="volunteer">An object of the logical entity type "volunteer" for update</param>
        void UpdateVolunteerDetails(int idRequester, BO.Volunteer volunteer);
        /// <summary>
        /// Requesting a request to the data layer to check if the volunteer can be deleted
        ///Attempting to request a deletion of the volunteer from the data layer
        /// </summary>
        /// <param name="idVolunteer">Volunteer ID</param>
        void DeleteVolunteer(int idVolunteer);
        /// <summary>
        /// From the logical object details, creates a new object of the data entity type DO.Voluteer
        ///Performs an attempt to request the addition of the new volunteer to the data layer(Create)
        /// </summary>
        /// <param name="newBoVolunteer">An object of the logical entity type "volunteer"</param>
        void AddVolunteer(BO.Volunteer newBoVolunteer);
    }
}

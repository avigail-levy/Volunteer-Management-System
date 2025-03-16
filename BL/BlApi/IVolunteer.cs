//using BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace BlApi
{
    public interface IVolunteer
    {
        BO.Role Login(string username);//לכאורה מיותר בגלל היוזיג של BO למעלה
        IEnumerable <BO.VolunteerInList> GetListVolunteers(bool? active , BO.VolunteerInListAttributes? filterByAttribute);// במימוש לכתוב בכותרת =נאל ע"פ הדרישות
        BO.Volunteer GetVolunteerDetails (int idVolunteer);
        void UpdateVolunteerDetails(int idVolunteer, BO.Volunteer volunteer);
        void DeleteVolunteer(int idVolunteer);
        void AddVolunteer(BO.Volunteer newBoVolunteer);





    }
}

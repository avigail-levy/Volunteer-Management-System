//using BO;
//using Microsoft.VisualBasic;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;
// למחוק את היוזינגים????????????????????????

namespace BlApi
{
    public interface ICall
    {
        int[] GetCallQuantitiesByStatus();
        IEnumerable<BO.CallInList> GetCallsList(BO.CallInListAttributes? filterByAttribute, object? filterValue, BO.CallInListAttributes? sortByAttribute);//null בכותרת המימוש
        BO.Call GetCallDitails(int idCall);
        void UpdateCallDetails(int idCall, BO.Call call);
        void DeleteCall(int idCall);
        void AddCall(BO.Call newBoCall);
        IEnumerable<BO.ClosedCallInList> ClosedCallsListHandledByVolunteer(int idVolunteer,BO.CallType? filterByAttribute, BO.ClosedCallInListAttributes? sortByAttribute);
        IEnumerable<BO.OpenCallInList> OpenCallsListSelectedByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.OpenCallInListAttributes? sortByAttribute);//null בכותרת המימוש
        void UpdateEndTreatmentOnCall(int idVolunteer, int idCallAssign);
        void UpdateCancelTreatmentOnCall(int id, int idCallAssign);
        void ChooseTreatmentCall(int idVolunteer,int idCall);
    }
}

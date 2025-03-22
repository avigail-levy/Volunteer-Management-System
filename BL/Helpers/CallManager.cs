using DalApi;

namespace Helpers
{
    internal static class CallManager
    {
        private static IDal s_dal = Factory.Get; //stage 4
        internal static void PeriodicCallsUpdates(DateTime oldClock, DateTime newClock) //stage 4
        {
            var list = s_dal.Call.ReadAll().ToList();
            foreach (var doCall in list)
            {
                //if student study for more than MaxRange years
                //then student should be automatically updated to 'not active'
                if (ClockManager.Now - doCall.OpeningTime >= s_dal.Config.RiskRange)
                {
                    //s_dal.Call.Update(doCall with { StatusCall = (StatusCall)OpenAtRisk });
                }
            }
        }
        internal static BO.StatusCall GetStatusCall(DO.Call call)
        {
            //רשימת הקצאות
            // ,InTreatment ,Closed ,Expired ,OpenAtRisk ,InTreatmentAtRisk 
            DateTime now = ClockManager.Now;
            IEnumerable<DO.Assignment> assignmentsCall = s_dal.Assignment.ReadAll(assignment => assignment.CallId == call.Id);
           
            if (!assignmentsCall
                .Where(a => (a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.CancelAdministrator)
                || (a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.SelfCancellation)
                && a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.Handled).Any()
                ||!assignmentsCall.Any())
                return BO.StatusCall.Open;
            if(assignmentsCall.Where(a=>(a.EntryTimeForTreatment<now&&n
            )))
            return BO.StatusCall.Open;
        }
    }
}

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
            DateTime now = ClockManager.Now;
            IEnumerable<DO.Assignment> assignmentsCall = s_dal.Assignment.ReadAll(assignment => assignment.CallId == call.Id);
            if (ClockManager.Now > call.MaxTimeFinishCall && assignmentsCall.Any(a => a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.Handled))
                return BO.StatusCall.Expired;//Expired
            if (assignmentsCall.Any(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled))
                return BO.StatusCall.Closed;//Closed
            if (assignmentsCall.Where(a => a.TypeOfTreatmentTermination == null).Any())
                if (now.Add(s_dal.Config.RiskRange) > call.MaxTimeFinishCall)
                    return BO.StatusCall.InTreatmentAtRisk;//InTreatmentAtRisk
                else return BO.StatusCall.InTreatment;//InTreatment
            if (now.Add(s_dal.Config.RiskRange) > call.MaxTimeFinishCall)
                return BO.StatusCall.OpenAtRisk;//OpenAtRisk
            else return BO.StatusCall.Open;//Open
        }

        internal static void validCall(BO.Call call)
        {

            if (call.MaxTimeFinishCall < call.OpeningTime)
            {
                throw new BO.BlInvalidValueException("the finish-time cant be earlier than the opening time");
            }
            if (!VolunteerManager.IsValidAddress(call.Longitude, call.Latitude))
            {
                throw new BO.BlInvalidValueException("Address not exist");
            };

        }




    }

}
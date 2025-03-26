using DalApi;
namespace Helpers
{
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get; //stage 4

        internal static DO.Assignment GetCallInTreatment(int idVol)
        {
            var assignVol = s_dal.Assignment.ReadAll(a => a.VolunteerId == idVol);
            DO.Assignment? assignInTreatment = (from a in assignVol
                                        let call = s_dal.Call.Read(a.CallId)
                                        where call != null &&
                                              (CallManager.GetStatusCall(call) == BO.StatusCall.InTreatment ||
                                               CallManager.GetStatusCall(call) == BO.StatusCall.InTreatmentAtRisk)
                                        select a).FirstOrDefault();
            return assignInTreatment;
        }
        internal static BO.StatusCallInProgress GetCallInProgress(DO.Call call)
        {
            return call.MaxTimeFinishCall-ClockManager.Now>s_dal.Config.RiskRange?
                BO.StatusCallInProgress.InTreatment:BO.StatusCallInProgress.InRiskTreatment;
        }
    }


}

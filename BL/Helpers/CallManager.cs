using DalApi;
using System.ComponentModel.Design;

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

            if (!assignmentsCall
                .Where(a => (a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.CancelAdministrator)
                || (a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.SelfCancellation)
                && a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.Handled).Any()
                || !assignmentsCall.Any())
                if (now.Add(s_dal.Config.RiskRange) > call.MaxTimeFinishCall)
                    return BO.StatusCall.OpenAtRisk;//OpenAtRisk
                else return BO.StatusCall.Open;//Open
            if (!assignmentsCall.Where(a => a.TypeOfTreatmentTermination != null).Any())
                if (now.Add(s_dal.Config.RiskRange) > call.MaxTimeFinishCall)
                    return BO.StatusCall.InTreatmentAtRisk;//InTreatmentAtRisk
                else return BO.StatusCall.InTreatment;//InTreatment
            if (assignmentsCall.Where(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled).Any())
                return BO.StatusCall.Closed;//Closed
            return BO.StatusCall.Expired;//Expired

        }

        internal static bool validCall(BO.Call call)
        {
            throw new NotImplementedException();
        }

        static double CalcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // רדיוס כדור הארץ בק"מ
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // המרחק בקילומטרים
        }

        static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

}


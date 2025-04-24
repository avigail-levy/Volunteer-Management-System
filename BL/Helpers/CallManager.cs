using DalApi;
using DO;

namespace Helpers
{
    internal static class CallManager
    {
        private static IDal s_dal = Factory.Get; //stage 4
        internal static void PeriodicCallsUpdates(DateTime oldClock, DateTime newClock) //stage 4
        {
            var listCalls = s_dal.Call.ReadAll().ToList();
            listCalls.Where(c=>ClockManager.Now - c.OpeningTime >= s_dal.Config.RiskRange)
            //foreach (var doCall in list)
            //{
            //    if (ClockManager.Now - doCall.OpeningTime >= s_dal.Config.RiskRange)
            //    {
            //        //s_dal.Call.Update(doCall with { StatusCall = (StatusCall)OpenAtRisk });
            //    }
            //    var calls = s_dal.Call.ReadAll().Where(c => c.MaxTimeFinishCall > ClockManager.Now
            //           && (GetStatusCall(c) != BO.StatusCall.Closed && GetStatusCall(c) != BO.StatusCall.Expired)).ToList();
            //var assin = from c in calls
            //            from a in s_dal.Assignment.ReadAll(a => a.CallId == c.Id)
            //            where a.TypeOfTreatmentTermination is null
            //            select a;

            //if (assin.Any())
            //    CreateDoAssignment(assin, DO.TypeOfTreatmentTermination.CancellationExpired);



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


        }
        internal static DO.Call CreateDoCall(BO.Call call, bool add = false)
        {
            double[]? latlon = VolunteerManager.CalcCoordinates(call.CallAddress) ?? throw new BO.BlInvalidValueException("invalid address");
            validCall(call);
            DateTime openingTime = add ? ClockManager.Now : call.OpeningTime;
            DO.Call doCall = new(
                call.Id,
                (DO.CallType)call.CallType,
                call.CallAddress,
                latlon[0],
                latlon[1],
                openingTime,
                call.CallDescription,
                call.MaxTimeFinishCall
            );
            return doCall;
        }

        internal static IEnumerable<DO.Call> FilterAndSortCalls(IEnumerable<DO.Call> calls, BO.CallType? filterByAttribute,
                                                                object? sortByAttributeObj)
        {
            calls = filterByAttribute != null ?
                  from c in calls
                  where c.CallType == (DO.CallType)filterByAttribute
                  select c
                  :
                  calls;

            if (sortByAttributeObj != null)
            {
                var propertySort = sortByAttributeObj?.GetType().GetProperty(sortByAttributeObj.ToString());
                calls = propertySort != null ?
                    from c in calls
                    orderby propertySort.GetValue(c, null)
                    select c
                    :
                    from c in calls
                    orderby c.Id
                    select c;

            }
            return calls;
        }

        internal static DO.Assignment CreateDoAssignment(DO.Assignment assignment, DO.TypeOfTreatmentTermination type)
        {
            return new(
                assignment.Id,
                assignment.CallId,
                assignment.VolunteerId,
                assignment.EntryTimeForTreatment,
                type,
                ClockManager.Now
                );
        }
    }
}
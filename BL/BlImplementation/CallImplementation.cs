using BlApi;
using BO;


namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddCall(Call newBoCall)
    {
        throw new NotImplementedException();
    }

    public void ChooseTreatmentCall(int idVolunteer, int idCall)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ClosedCallInList> ClosedCallsListHandledByVolunteer(int idVolunteer, CallType? filterByAttribute, ClosedCallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public void DeleteCall(int idCall)
    {//לבדוק אם היא בסטטוס פתוחה אם כן מותר למחוק###########################################
        try
        {
            var callsInAssignment = _dal.Assignment.ReadAll()
                              .Where(a => a.CallId == idCall)
                              .Select(a => a.CallId);
            if (callsInAssignment.Count()!=0)
                throw new BlCantDeleteException("It is not possible to delete a call handling assignment.");
            _dal.Call.Delete(idCall);
        }
        catch (Exception ex)
        {
            throw new BlCantDeleteException("It is not possible to delete the call", ex);
        }
    }

    public BO.Call GetCallDetails(int idCall)
    {//#####################################################################################
        DO.Call call = _dal.Call.Read(idCall)??throw new BlDoesNotExistException("call does not exist");
        Call newBOCall = new Call
        {
            Id = call.Id,
            CallAddress = call.CallAddress,
            CallDescription = call.CallDescription,
            CallType = (BO.CallType)call.CallType,
            MaxTimeFinishCall = call.MaxTimeFinishCall,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            StatusCall =,
            AssignmentListForCall = 
            };
        
        return new BO.Call();
    }

    public int[] GetCallQuantitiesByStatus()
    {
        //######################################################################################
        var calls = _dal.Call.ReadAll();
        var callsByStatus = calls.GroupBy(call => call.CallAddress);
        return [];
    }

    public IEnumerable<CallInList> GetCallsList(CallInListAttributes? filterByAttribute, object? filterValue, CallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OpenCallInList> OpenCallsListSelectedByVolunteer(int idVolunteer, CallType? filterByAttribute, OpenCallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallDetails(Call call)
    {
        throw new NotImplementedException();
    }

    public void UpdateCancelTreatmentOnCall(int id, int idCallAssign)
    {
        throw new NotImplementedException();
    }

    public void UpdateEndTreatmentOnCall(int idVolunteer, int idCallAssign)
    {
        throw new NotImplementedException();
    }
}

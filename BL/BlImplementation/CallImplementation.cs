using BlApi;
using Helpers;

namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddCall(BO.Call newBoCall)
    {
        throw new NotImplementedException();
    }

    public void ChooseTreatmentCall(int idVolunteer, int idCall)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.ClosedCallInList> ClosedCallsListHandledByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.ClosedCallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public void DeleteCall(int idCall)
    {
        try
        {
            var callsInAssignment = _dal.Assignment.ReadAll()
                              .Where(a => a.CallId == idCall)
                              .Select(a => a.CallId);
            if (callsInAssignment.Count()!=0)
                throw new BO.BlCantDeleteException("It is not possible to delete a call handling assignment.");
            DO.Call call= _dal.Call.Read(idCall);
            if (CallManager.GetStatusCall(call) != BO.StatusCall.Open)
                throw new BO.BlCantDeleteException("It is not possible to delete a call that is not open.");
            _dal.Call.Delete(idCall);
        }
        catch (Exception ex)
        {
            throw new BO.BlCantDeleteException("It is not possible to delete the call", ex);
        }
    }

    public BO.Call GetCallDetails(int idCall)
    {//#####################################################################################
        DO.Call call = _dal.Call.Read(idCall) ?? throw new BO.BlDoesNotExistException("call does not exist");
        BO.Call newBOCall = new BO.Call
        {
            Id = call.Id,
            CallAddress = call.CallAddress,
            CallDescription = call.CallDescription,
            CallType = (BO.CallType)call.CallType,
            MaxTimeFinishCall = call.MaxTimeFinishCall,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            StatusCall =CallManager.GetStatusCall(call),
            //AssignmentListForCall = 
            };
        
        return newBOCall;
    }

    public int[] GetCallQuantitiesByStatus()
    {
        //######################################################################################
        var calls = _dal.Call.ReadAll();
        var callsByStatus = calls.GroupBy(call => CallManager.GetStatusCall(call));
        return [];
    }

    public IEnumerable<BO.CallInList> GetCallsList(BO.CallInListAttributes? filterByAttribute, object? filterValue, BO.CallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.OpenCallInList> OpenCallsListSelectedByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.OpenCallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallDetails(BO.Call call)
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

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
    {
        throw new NotImplementedException();
    }

    public Call GetCallDetails(int idCall)
    {
        throw new NotImplementedException();
    }

    public int[] GetCallQuantitiesByStatus()
    {
        throw new NotImplementedException();
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

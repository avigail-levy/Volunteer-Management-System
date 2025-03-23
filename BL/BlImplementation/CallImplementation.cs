using BlApi;
using BO;
using Helpers;
using System.Net.Security;

namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    //###########################################בדיקות תקינות
    public void AddCall(BO.Call newBoCall)
    {
        try
        {
            if (!CallManager.validCall(newBoCall))
            {
                throw new BlInvalidValueException("invalid values");
            }
            DO.Call doCall =
             new(newBoCall.Id,
                 (DO.CallType)newBoCall.CallType,
                 newBoCall.CallAddress,
                 newBoCall.Latitude,
                 newBoCall.Longitude,
                 newBoCall.OpeningTime,
                 newBoCall.CallDescription,
                 newBoCall.MaxTimeFinishCall
                 );

            _dal.Call.Create(doCall);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"Call with ID={newBoCall.Id} already exists", ex);
        }
    }

    public void ChooseTreatmentCall(int idVolunteer, int idCall)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.ClosedCallInList> ClosedCallsListHandledByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.ClosedCallInListAttributes? sortByAttribute)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// delete call
    /// </summary>
    /// <param name="idCall">call id</param>
    /// <exception cref="BO.BlCantDeleteException">if it is not possible to delete</exception>
    public void DeleteCall(int idCall)
    {
        try
        {
            var callsInAssignment = _dal.Assignment.ReadAll()
                              .Where(a => a.CallId == idCall)
                              .Select(a => a.CallId);
            if (callsInAssignment.Count() != 0)
                throw new BO.BlCantDeleteException("It is not possible to delete a call handling assignment.");
            DO.Call call = _dal.Call.Read(idCall);
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
            StatusCall = CallManager.GetStatusCall(call),
            CallAssignInList = _dal.Assignment.ReadAll(a => a.CallId == idCall)
                                                               .Select(a => new BO.CallAssignInList
                                                               {
                                                                   VolunteerId = a.VolunteerId,
                                                                   Name = _dal.Volunteer.Read(v => v.Id == a.VolunteerId).Name,
                                                                   EntryTimeForTreatment = a.EntryTimeForTreatment,
                                                                   EndOfTreatmentTime = a.EndOfTreatmentTime,
                                                                   TypeOfTreatmentTermination = (BO.TypeOfTreatmentTermination)a.TypeOfTreatmentTermination
                                                               }).ToList()
        };
        return newBOCall;
    }

/// <summary>
/// In each cell in the array at index i, the number of calls whose status value is equal to i will be counted.
/// </summary>
/// <returns>Returns an array of quantities according to the call status</returns>
public int[] GetCallQuantitiesByStatus()
{
    int[] callCounts = new int[Enum.GetValues(typeof(BO.StatusCall)).Length];//size of array sach as num of option 
    var calls = _dal.Call.ReadAll();
    var callsByStatus = calls.GroupBy(call => CallManager.GetStatusCall(call))
        .ToDictionary(group => (int)group.Key, group => group.Count());
    foreach (var group in callsByStatus)
    {
        callCounts[group.Key] = group.Value;
    }
    return callCounts;
}

public IEnumerable<BO.CallInList> GetCallsList(BO.CallInListAttributes? filterByAttribute=null, object? filterValue=null, BO.CallInListAttributes? sortByAttribute=null)
{
        throw new NotImplementedException();

    }

    public IEnumerable<BO.OpenCallInList> OpenCallsListSelectedByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.OpenCallInListAttributes? sortByAttribute)
{
    throw new NotImplementedException();
}

public void UpdateCallDetails(BO.Call call)
{/////////////////////////////########################בדיקות תקינות
    try
    {
        if (!CallManager.validCall(call))
        {
            throw new BlInvalidValueException("invalid values");
        }
        DO.Call doCall =
         new(call.Id,
             (DO.CallType)call.CallType,
             call.CallAddress,
             call.Latitude,
             call.Longitude,
             call.OpeningTime,
             call.CallDescription,
             call.MaxTimeFinishCall
             );

        _dal.Call.Update(doCall);
    }
    catch (DO.DalDoesNotExistException ex)
    {
        throw new BO.BlDoesNotExistException($"Call with ID={call.Id} is not exists", ex);
    }

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

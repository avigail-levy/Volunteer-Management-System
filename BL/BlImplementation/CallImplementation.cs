using BlApi;
using Helpers;
namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddCall(BO.Call newBoCall)
    {
        CallManager.validCall(newBoCall);
        try
        {
            DO.Call doCall = new(
                 newBoCall.Id,
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
            throw new BO.BlAlreadyExistsException($"Call with ID={newBoCall.Id} already exists", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idVolunteer"></param>
    /// <param name="idCall"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlInvalidRequestException"></exception>
    public void ChooseTreatmentCall(int idVolunteer, int idCall)
    {
        var call = _dal.Call.Read(idCall) ?? throw new BO.BlDoesNotExistException($"the call with id{idCall} does not exist");
        if (CallManager.GetStatusCall(call) != BO.StatusCall.Open)
            throw new BO.BlInvalidRequestException($"The call with id{idCall} cannot be taken, The call status is not open");
        try
        {
            DO.Assignment newAssignment = new(
                 1,
                 idCall,
                 idVolunteer,
                 ClockManager.Now
                );
            _dal.Assignment.Create(newAssignment);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlInvalidRequestException($"The call with id{idCall} cannot be taken", ex);

        }
    }
    public IEnumerable<BO.ClosedCallInList> ClosedCallsListHandledByVolunteer(int idVolunteer, BO.CallType? filterByAttribute = null, BO.ClosedCallInListAttributes? sortByAttribute = null)
    {//###################################################
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        var assignments = _dal.Assignment.ReadAll();
        //הקריאות הסגורות של אותו מתנדב
        var volCalls = from c in calls
                       from a in assignments
                       where c.Id == a.CallId && a.VolunteerId == idVolunteer && CallManager.GetStatusCall(c) == BO.StatusCall.Closed
                       select c;

        volCalls = (filterByAttribute != null) ?
                   from c in volCalls
                   where c.CallType == (DO.CallType)filterByAttribute
                   select c
                   : volCalls;

        volCalls = sortByAttribute != null ?
                  from c in calls
                  orderby sortByAttribute
                  select c
                  :
                  from c in calls
                  orderby c.Id
                  select c;

        return volCalls.Select(c => new BO.ClosedCallInList
        {
            Id = c.Id,
            CallType = (BO.CallType)c.CallType,
            CallAddress = c.CallAddress,
            OpeningTime = c.OpeningTime,
            EntryTimeForTreatment = _dal.Assignment.Read(c.Id).EntryTimeForTreatment,
            EndOfTreatmentTime = _dal.Assignment.Read(c.Id).EndOfTreatmentTime,
            TypeOfTreatmentTermination = (BO.TypeOfTreatmentTermination)_dal.Assignment.Read(c.Id).TypeOfTreatmentTermination
        });
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
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantDeleteException("It is not possible to delete the call", ex);
        }
    }
    /// <summary>
    /// Requests the data layer to get details about the call and its allocation list.
    /// </summary>
    /// <param name="idCall">id call</param>
    /// <returns>List of logical entities of type "List Read Assignment"</returns>
    /// <exception cref="BO.BlDoesNotExistException">call does not exist</exception>
    public BO.Call GetCallDetails(int idCall)
    {
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
    /// <summary>
    /// Returns a sorted and filtered collection of entities "call  in a list"
    /// </summary>
    /// <param name="filterByAttribute">A field in the "callInList" entity by which the list will be filtered</param>
    /// <param name="filterValue">Value to filter</param>
    /// <param name="sortByAttribute">a field in the "List Read" entity, by which the list is sorted</param>
    /// <returns></returns>
    public IEnumerable<BO.CallInList> GetCallsList(BO.CallInListAttributes? filterByAttribute = null, object? filterValue = null, BO.CallInListAttributes? sortByAttribute = null)
    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        var propertyFilter = typeof(DO.Call).GetProperty(filterByAttribute.ToString());

        calls = propertyFilter != null ?
                from c in calls
                where propertyFilter.GetValue(c, null) == filterValue
                select c
                :
                from item in calls
                select item;


        var propertySort = typeof(DO.Call).GetProperty(sortByAttribute.ToString());

        calls = sortByAttribute != null ?
                from c in calls
                orderby propertySort.GetValue(c, null)
                select c
                :
                from c in calls
                orderby c.Id
                select c;

        return calls.Select(c =>
        {
            var allAssign = _dal.Assignment.ReadAll(a => a.CallId == c.Id);
            return new BO.CallInList
            {
                Id = 1,
                CallId = c.Id,
                CallType = (BO.CallType)c.CallType,
                OpeningTime = c.OpeningTime,
                TotalTimeRemainingFinishCalling = c.MaxTimeFinishCall - ClockManager.Now,
                LastVolunteerName = _dal.Volunteer.Read(allAssign.LastOrDefault()?.VolunteerId ?? 0)!.Name,
                TotalTimeCompleteTreatment = allAssign.LastOrDefault()?.TypeOfTreatmentTermination
            == DO.TypeOfTreatmentTermination.Handled ? allAssign.LastOrDefault()?.EndOfTreatmentTime - c.OpeningTime : null,
                StatusCall = CallManager.GetStatusCall(c),
                TotalAssignments = allAssign.Count()
            };
        });
    }
    /// <summary>
    /// The collection will include - all readings with the status "open" or "open at risk"
    /// </summary>
    /// <param name="idVolunteer">Volunteer ID - for whom the list of open calls for selection and 
    /// their distance from their current distance is returned</param>
    /// <param name="filterByAttribute">The ENUM value of the call type by which the list will be filtered.</param>
    /// <param name="sortByAttribute">A parameter that is an ENUM value of a field in the "Open Read in List" 
    /// entity, by which the list is sorted.</param>
    /// <returns>A sorted collection of a logical data entity "Open Reads in List" that includes the distance
    /// of each read from the volunteer</returns>
    public IEnumerable<BO.OpenCallInList> OpenCallsListSelectedByVolunteer(int idVolunteer, BO.CallType? filterByAttribute, BO.OpenCallInListAttributes? sortByAttribute)
    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();

        var openCalls = from c in calls
                        where CallManager.GetStatusCall(c) == BO.StatusCall.Open || CallManager.GetStatusCall(c) == BO.StatusCall.OpenAtRisk
                        select c;

        openCalls = filterByAttribute != null ?
                    from c in calls
                    where c.CallType == (DO.CallType)filterByAttribute
                    select c
                    :
                    openCalls;

        openCalls = sortByAttribute != null ?
                  from c in calls
                  orderby sortByAttribute
                  select c
                  :
                  from c in calls
                  orderby c.Id
                  select c;

        var vol = _dal.Volunteer.Read(idVolunteer) ??
            throw new BO.BlDoesNotExistException($"The volunteer with id:{idVolunteer} does not exist");
        return openCalls.Select(c => new BO.OpenCallInList
        {
            Id = c.Id,
            CallType = (BO.CallType)c.CallType,
            CallDescription = c.CallDescription,
            CallAddress = c.CallAddress,
            OpeningTime = c.OpeningTime,
            MaxTimeFinishCall = c.MaxTimeFinishCall,
            CallingDistanceFromTreatingVolunteer = Tools.CalcDistance(c.Latitude, c.Longitude, vol.Latitude, vol.Longitude)

        });
    }
    /// <summary>
    /// update call details
    /// </summary>
    /// <param name="call">Object of the logical entity type "Call" BO.Call</param>
    /// <exception cref="BO.BlInvalidValueException">Invalid value exception</exception>
    /// <exception cref="BO.BlDoesNotExistException">Call does not exist exception</exception>
    public void UpdateCallDetails(BO.Call call)
    {
        CallManager.validCall(call);
        try
        {

            DO.Call doCall = new(

                 call.Id,
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
    /// <summary>
    /// "Cancel Handling" update method on read
    /// </summary>
    /// <param name="id">ID of the person requesting the cancellation request</param>
    /// <param name="idCallAssign">assignment id</param>
    /// <exception cref="BlUnauthorizedException">No permission to update cancellation</exception>
    /// <exception cref="BlCantUpdateEception">Error during update</exception>
    public void UpdateCancelTreatmentOnCall(int idRequest, int idAssign)
    {
        try
        {
            DO.Assignment assignment = _dal.Assignment.Read(idAssign) ?? throw new BO.BlDoesNotExistException($"Assignment with id:{idAssign} does not exist");
            DO.Call call = _dal.Call.Read(assignment!.CallId);
            if (!(CallManager.GetStatusCall(call) == BO.StatusCall.Open))
                throw new BO.BlCantUpdateException("the call is not open");
            if (_dal.Volunteer.Read(idRequest)!.Role != DO.Role.Manager)
                if (assignment.VolunteerId != idRequest)
                    throw new BO.BlUnauthorizedException("You are not allowed to update the call.");
            DO.Assignment newAssignment = new(
                assignment.Id,
                assignment.CallId,
                assignment.VolunteerId,
                assignment.EntryTimeForTreatment,
                assignment.VolunteerId == idRequest ? DO.TypeOfTreatmentTermination.SelfCancellation
                : DO.TypeOfTreatmentTermination.CancelAdministrator,
                ClockManager.Now
                );
            _dal.Assignment.Update(newAssignment);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantUpdateException("Unable to update the assignment", ex);
        }
    }
    /// <summary>
    /// "EndTreatment" update method on read
    /// </summary>
    /// <param name="id">ID of the person requesting the cancellation request</param>
    /// <param name="idCallAssign">assignment id</param>
    /// <exception cref="BlUnauthorizedException">No permission to update cancellation</exception>
    /// <exception cref="BlCantUpdateEception">Error during update</exception>
    public void UpdateEndTreatmentOnCall(int idVolunteer, int idAssign)
    {
        try
        {
            DO.Assignment assignment = _dal.Assignment.Read(idAssign) ?? throw new BO.BlDoesNotExistException($"Assignment with id:{idAssign} does not exist");
            DO.Call call = _dal.Call.Read(assignment.CallId);
            if (!(CallManager.GetStatusCall(call) == BO.StatusCall.Open))
                throw new BO.BlCantUpdateException("the call is not open");
            if (assignment.VolunteerId != idVolunteer)
                throw new BO.BlUnauthorizedException("You are not allowed to update the call.");
            DO.Assignment newAssignment = new(
                assignment.Id,
                assignment.CallId,
                assignment.VolunteerId,
                assignment.EntryTimeForTreatment,
                DO.TypeOfTreatmentTermination.Handled,
                ClockManager.Now);
            _dal.Assignment.Update(newAssignment);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlCantUpdateException("Unable to update the assignment", ex);
        }
    }
}

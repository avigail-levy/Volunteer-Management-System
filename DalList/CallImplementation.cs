namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int callId = Config.NextCallId;
        Call newCall=item with { Id = callId };
        DataSource.Calls.Add(newCall);
    }

    public void Delete(int id)
    {
        Call? callToRemove=DataSource.Calls.Find(call=>call.Id == id);
        if (callToRemove != null) 
            DataSource.Calls.Remove(callToRemove);
            
        else
            throw new Exception($"Call with ID={id} is not exists");
    }

    public void DeleteAll()
    {
        DataSource.Calls.Clear();   
    }

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(call => call.Id == id);
    }

    public List<Call> ReadAll()
    {
        return new List<Call>(DataSource.Calls);
    }

    public void Update(Call item)
    {
        Call? updateCall = DataSource.Calls.Find(call => call.Id == item.Id);
        if (updateCall != null)
        {
            DataSource.Calls.Remove(updateCall);
            DataSource.Calls.Add(updateCall);
        }
        else
        {
            throw new Exception($"Call with ID={item.Id} is not exists");
        }
    }
}

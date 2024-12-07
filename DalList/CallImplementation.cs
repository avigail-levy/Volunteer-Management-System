namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class CallImplementation : ICall
{/// <summary>
/// מתודה להוספת קריאה חדשה 
/// </summary>
/// <param name="item">אובייקט קיראה להוספת</param>
    public void Create(Call item)
    {
        int callId = Config.NextCallId;
        Call newCall=item with { Id = callId };
        DataSource.Calls.Add(newCall);
    }
    /// <summary>
    /// מתודה למחיקת קיראה ע"פ מספר שלה
    /// </summary>
    /// <param name="id">מספר קריאה</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Call? callToRemove=DataSource.Calls.Find(call=>call.Id == id);
        if (callToRemove != null) 
            DataSource.Calls.Remove(callToRemove);
            
        else
            throw new Exception($"Call with ID={id} is not exists");
    }
    // מתודה למחיקת כל הקריאות
    public void DeleteAll()
    {
        DataSource.Calls.Clear();   
    }

    /// <summary>
    ///מתודה לקריאת הקריאה ע"פ מספר זיהוי 
    /// </summary>
    /// <param name="id">מספר זיהוי של קריאה</param>
    /// <returns></returns>
    public Call? Read(int id)
    {
        return DataSource.Calls.Find(call => call.Id == id);
    }

    /// <summary>
    /// method to return data.
    /// </summary>
    /// <param name="filter">boolian function to filter the data to be returned</param>
    /// <returns>one data</returns>
    public Call Read(Func<Call, bool>? filter)
     => DataSource.Calls.FirstOrDefault(filter);

    /// <summary>
    /// method to return data.
    /// </summary>
    /// <param name="filter">boolian function to filter the data to be returned</param>
    /// <returns>all or part of the data</returns>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
       => filter == null
          ? DataSource.Calls.Select(item => item)
           : DataSource.Calls.Where(filter);
    /// <summary>
    /// מתודה לעדכון קריאה ע"פ המספר שלה
    /// </summary>
    /// <param name="item">אובייקט קריאה לעדכון</param>
    /// <exception cref="Exception"></exception>
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

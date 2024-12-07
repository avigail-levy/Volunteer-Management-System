namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{/// <summary>
/// מתודה להוספת מתנדב חדש
/// </summary>
/// <param name="item"></param>
/// <exception cref="Exception"></exception>
    public void Create(Volunteer item)
    {
        Volunteer? createVolunteer = DataSource.Volunteers.Find(volunteer => volunteer.Id == item.Id);
        if (createVolunteer == null)
        {
            createVolunteer = item;
            DataSource.Volunteers.Add(createVolunteer);
        }
        else
        {
            throw new Exception($"Volunteer with ID={item.Id} already exists");
        }

    }
    /// <summary>
    /// מתודה למחיקת מתנדב ע"פ הת.ז שלו 
    /// </summary>
    /// <param name="id">ID - האובייקט למחיקה</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Volunteer? volunteerToRemove = DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
        if (volunteerToRemove != null)
            DataSource.Volunteers.Remove(volunteerToRemove);

        else
            throw new Exception($"Volunteer with ID={id} is not exists");
    }
   //מתודה למחיקת כל המתנדבים
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }
    /// <summary>
    /// מתודה לקריאת מתנדב ע"פ הת.ז שלו 
    /// </summary>
    /// <param name="id">ת.ז של מתנדב לקריאת פרטיו</param>
    /// <returns></returns>

    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
    }

    /// <summary>
    /// method to return data.
    /// </summary>
    /// <param name="filter">boolian function to filter the data to be returned</param>
    /// <returns>one data</returns>
    public Volunteer Read(Func<Volunteer, bool>? filter)
     => DataSource.Volunteers.FirstOrDefault(filter);

    /// <summary>
    /// method to return data.
    /// </summary>
    /// <param name="filter">boolian function to filter the data to be returned</param>
    /// <returns>all or part of the data</returns>
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) //stage 2
      => filter == null
         ? DataSource.Volunteers.Select(item => item)
          : DataSource.Volunteers.Where(filter);

    /// <summary>
    /// מתודה לעידכון פרטי מתנדב ע"פ ת.ז שלו
    /// </summary>
    /// <param name="item">מתנדב לעדכון</param>
    /// <exception cref="Exception"></exception>
    public void Update(Volunteer item)
    {
        Volunteer? updateVolunteer = DataSource.Volunteers.Find(volunteer => volunteer.Id == item.Id);
        if (updateVolunteer != null)
        {
            DataSource.Volunteers.Remove(updateVolunteer);
            DataSource.Volunteers.Add(item);
        }
        else
        {
            throw new Exception($"Volunteer with ID={item.Id} is not exists");
        }
    }
}

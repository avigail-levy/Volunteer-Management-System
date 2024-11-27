namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class VolunteerImplementation : IVolunteer
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
    //מתודה לקריאת כל המתנדבים
    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }
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

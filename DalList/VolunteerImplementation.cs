namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class VolunteerImplementation : IVolunteer
{
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

    public void Delete(int id)
    {
        Volunteer? volunteerToRemove = DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
        if (volunteerToRemove != null)
            DataSource.Volunteers.Remove(volunteerToRemove);

        else
            throw new Exception($"Volunteer with ID={id} is not exists");
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
    }

    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }

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

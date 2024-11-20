namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int assignmentId = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = assignmentId };
        DataSource.Assignments.Add(newAssignment);
    }

    public void Delete(int id)
    {
        Assignment? assignmentToRemove = DataSource.Assignments.Find(assignment => assignment.Id == id);
        if (assignmentToRemove != null)
            DataSource.Assignments.Remove(assignmentToRemove);

        else
            throw new Exception($"Assignment with ID={id} is not exists");
    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(assignment => assignment.Id == id);
    }

    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }

    public void Update(Assignment item)
    {
        Assignment? updateAssignment = DataSource.Assignments.Find(assignment => assignment.Id == item.Id);
        if (updateAssignment != null)
        {
            DataSource.Assignments.Remove(updateAssignment);
            DataSource.Assignments.Add(updateAssignment);
        }
        else
        {
            throw new Exception($"Assignment with ID={item.Id} is not exists");
        }
    }
}

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// מתודה ליצירת הקצאה חדשה 
    /// </summary>
    /// <param name="item">הקצאה חדשה להוספה</param>
    public void Create(Assignment item)
    {
        int assignmentId = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = assignmentId };
        DataSource.Assignments.Add(newAssignment);
    }
    /// <summary>
    /// מתודה למחיקת הקצאה ע"פ מספרה
    /// </summary>
    /// <param name="id">מספר הקצאה</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Assignment? assignmentToRemove = DataSource.Assignments.Find(assignment => assignment.Id == id);
        if (assignmentToRemove != null)
            DataSource.Assignments.Remove(assignmentToRemove);

        else
            throw new Exception($"Assignment with ID={id} is not exists");
    }
    //מתודה למחיקת כל ההקצאות
    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }
    /// <summary>
    /// מתודה לקריאת הקצאה ע"פ מספרה
    /// </summary>
    /// <param name="id">מספר הקצאה</param>
    /// <returns></returns>
    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(assignment => assignment.Id == id);
    }
    // מתודה לקריאת כל ההקצאות
    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }
    /// <summary>
    ///  מתודה לעדכון הקצאה ע"פ המספר שלה
    /// </summary>
    /// <param name="item">אובייקט הקצאה לעדכון</param>
    /// <exception cref="Exception"></exception>
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

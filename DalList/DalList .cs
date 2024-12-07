namespace Dal;
using DalApi;
sealed public class DalList : IDal
{
    public IVolunteer Volunteer { get; }= new VolunteerImplementation();
    public ICall Call { get; } = new CallImplementation();

    public IAssignment Assignment { get; } = new AssignmentImplementation();

    public IConfig Config { get; } = new ConfigImplementation();
    /// <summary>
    ///reset all entities
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();	  
        Config.Reset();
    }
}


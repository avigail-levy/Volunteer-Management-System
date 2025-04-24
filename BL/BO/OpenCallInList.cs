namespace BO;
using Helpers;
public class OpenCallInList
{
    /// <summary>
    /// Call ID :int, Automatic 
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Call type: BO.CallType
    /// </summary>
    public CallType CallType { get; set; }
    /// <summary>
    /// Call description: string
    /// </summary>
    public string? CallDescription { get; set; }
    /// <summary>
    /// Call address : string
    /// </summary>
    public string CallAddress { get; set; }
    /// <summary>
    /// Call opening time: DateTime
    /// </summary>
    public DateTime OpeningTime { get; set; }
    /// <summary>
    /// Maximum reading completion time: DateTime
    /// </summary>
    public DateTime? MaxTimeFinishCall { get; set; }
    /// <summary>
    /// Call distance from the volunteer who is caring for her
    /// </summary>
    public double CallingDistanceFromTreatingVolunteer { get; set; }
    public override string ToString() => this.ToStringProperty();

}

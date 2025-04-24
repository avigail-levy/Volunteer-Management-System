namespace BO;
using Helpers;

public class Call
{/// <summary>
/// ID: Automatic identification number
/// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Call type : BO.CallType
    /// </summary>
    public CallType CallType { get; set; }
    /// <summary>
    ///call description: string
    /// </summary>
    public string? CallDescription { get; set; }
    /// <summary>
    ///Call address:string
    /// </summary>
    public string CallAddress { get; set; }
    /// <summary>
    /// Latitude: will be calculated automatically
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// Longitude: will be calculated automatically
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// Reading start time: DateTime
    /// </summary>
    public DateTime OpeningTime { get; init; }
    /// <summary>
    /// Maximum time to finish call: DateTime
    /// </summary>
    public DateTime? MaxTimeFinishCall {  get; set; }
    /// <summary>
    /// Call status: BO.StatusCall
    /// </summary>
    public StatusCall StatusCall { get; set; }
    /// <summary>
    ///Assignment list to call
    ///If there are no allocations it will be NULL
    ///Type BO.Assignment
    /// </summary>
    public List<BO.CallAssignInList>? CallAssignInList { get; set; }

    public override string ToString() => this.ToStringProperty();

}

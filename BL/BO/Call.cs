namespace BO;
using Helpers;

public class Call
{/// <summary>
/// ID: Automatic identification number
/// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Call type : BO.CallType
    /// call description: string
    /// Call address:string
    /// Latitude: will be calculated automatically
    /// Longitude: will be calculated automatically
    /// Reading start time: DateTime
    /// Maximum time to finish call: DateTime
    /// 
    /// Call status: BO.StatusCall
    /// Assignment list to call
    /// If there are no allocations it will be NULL
    /// Type BO.Assignment
    /// </summary>
    public CallType CallType { get; set; }
    public string? CallDescription { get; set; }
    public string CallAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime OpeningTime { get; init; }
    public DateTime? MaxTimeFinishCall {  get; set; }
    public StatusCall StatusCall { get; set; }
    public List<BO.CallAssignInList>? CallAssignInList { get; set; }

    public override string ToString() => this.ToStringProperty();

}

namespace BO;
using Helpers;
public class CallInList
{
    public int? Id {  get; init; }
    public int CallId { get; init; }
    public CallType CallType { get; set; }
    public DateTime OpeningTime { get; set; }
    public TimeSpan? TotalTimeRemainingFinishCalling {  get; set; }
    public string? LastVolunteerName { get; set; }
    public TimeSpan? TotalTimeCompleteTreatment {  get; set; }
    public StatusCall StatusCall { get; set; }
    public int TotalAssignments { get; set; }

   public override string ToString() => this.ToStringProperty();
}

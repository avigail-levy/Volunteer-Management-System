namespace BO;
using Helpers;
public class CallInProgress
{
    
    public int Id {  get; init; }
    public int CallId {  get; init; }
    public CallType CallType { get; set; }
    public string? CallDescription{ get; set; }
    public string CallAddress { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime? MaxTimeFinishCall {  get; set; }
    public DateTime EntryTimeForTreatment { get; set; }//maybee init??
    public double CallingDistanceFromTreatingVolunteer { get; set; }
    public StatusCallInProgress StatusCalling { get; set; }

    public override string ToString() => this.ToStringProperty();
}

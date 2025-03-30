namespace BO;
using Helpers;

public class CallAssignInList
{
    public int? VolunteerId { get; init; }
    public string? Name { get; set; }
    public DateTime EntryTimeForTreatment { get; set; }
    public DateTime? EndOfTreatmentTime {  get; set; }
    public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
    public override string ToString() => this.ToStringProperty();


}


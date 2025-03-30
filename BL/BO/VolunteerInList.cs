namespace BO;
using Helpers;
public class VolunteerInList
{
    public int Id {  get; init; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public int TotalCallsHandledByVolunteer { get; set; }
    public int TotalCallsCanceledByVolunteer { get; set; }
    public int TotalExpiredCallingsByVolunteer { get; set; }
    public int? IDCallInHisCare { get; set; }
    public CallType CallType { get; set; }

    public override string ToString() =>this.ToStringProperty();

}

namespace BO
{
    public class OpenCallInList
    {
        public int Id { get; init; }
        public CallType CallType { get; set; }
        public string? CallDescription { get; set; }//תיאור מילולי של הקריאה
        public string CallAddress { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime? MaxTimeFinishCall { get; set; }
        public double CallingDistanceFromTreatingVolunteer { get; set; }

        public override string ToString() => this.ToStringProperty();

    }
}

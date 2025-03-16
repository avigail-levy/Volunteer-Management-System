namespace BO
{
    public class CallInProgress
    {
        //ישות לצפיה בלבד ולכן אין בדיקות תקינות
        public int Id {  get; init; }
        public int CallId {  get; init; }
        public CallType CallType { get; set; }
        public string? CallDescription{ get; set; }//תיאור מילולי של הקריאה
        public string CallAddress { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime? MaxTimeFinishCall {  get; set; }
        public DateTime EntryTimeForTreatment { get; set; }//maybee init??
        public double CallingDistanceFromTreatingVolunteer { get; set; }
        public StatusCalling StatusCalling { get; set; }

        //public override string ToString() => this.ToStringProperty();
    }
}

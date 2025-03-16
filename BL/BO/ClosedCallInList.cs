namespace BO
{
    public class ClosedCallInList
    {
        //ישות לצפייה בלבד
        public int Id { get; init; }
        public CallType CallType { get; set; }
        public string CallAddress { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime EntryTimeForTreatment { get; set;}
        public DateTime? EndOfTreatmentTime { get; set; }
        public TypeOfTreatmentTermination? TypeOfTreatmentTermination { get; set; }

        //public override string ToString() => this.ToStringProperty();


    }
}

namespace BO
{
    public class Call
    {
        public int Id { get; init; }
        public CallType CallType { get; set; }
        public string? CallDescription { get; set; }//תיאור מילולי של הקריאה
        public string? CallAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime OpeningTime { get; init; }
        public DateTime? MaxTimeFinishCall {  get; set; }
        public StatusCall StatusCall { get; set; }
        public List<BO.CallAssignInList>? AssignmentListForCall { get; set; }//כאשר אין ברשימה הקצאות לבדוק לפני כן שלא NULL
        // כי לא בקשו לאתחל לרשימה ריקה אלא NULL

        //public override string ToString() => this.ToStringProperty();

    }
}

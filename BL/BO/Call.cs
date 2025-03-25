namespace BO
{
    public class Call
    {
        private List<CallAssignInList> callAssignInLists;

        public Call(int id, string callAddress, string? callDescription, CallType callType, DateTime? maxTimeFinishCall, double latitude, double longitude, DateTime openingTime, StatusCall statusCall, List<CallAssignInList> callAssignInLists)
        {
            Id = id;
            CallAddress = callAddress;
            CallDescription = callDescription;
            CallType = callType;
            MaxTimeFinishCall = maxTimeFinishCall;
            Latitude = latitude;
            Longitude = longitude;
            OpeningTime = openingTime;
            StatusCall = statusCall;
            this.callAssignInLists = callAssignInLists;
        }

        public int Id { get; init; }
        public CallType CallType { get; set; }
        public string? CallDescription { get; set; }//תיאור מילולי של הקריאה
        public string CallAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime OpeningTime { get; init; }
        public DateTime? MaxTimeFinishCall {  get; set; }
        public StatusCall StatusCall { get; set; }
        public List<BO.CallAssignInList>? CallAssignInList { get; set; }//כאשר אין ברשימה הקצאות לבדוק לפני כן שלא NULL
        // כי לא בקשו לאתחל לרשימה ריקה אלא NULL

        //public override string ToString() => this.ToStringProperty();

    }
}

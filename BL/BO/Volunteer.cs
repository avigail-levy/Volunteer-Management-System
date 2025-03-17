namespace BO   
{
    /// <summary>
    /// לכתוב תיעוד לכל התכונות!!!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    public class Volunteer
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Address {  get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Role Role { get; set; }
        public bool Active { get; set; }
        public double? MaxDistanceForCall { get; set; }
        public DistanceType DistanceType { get;set; }
        public int TotalCallsHandled { get;set;}
        public int TotalCallsCanceled { get; set; }
        public int TotalCallsChoseHandleHaveExpired { get; set; }
        public BO.CallInProgress? CallingVolunteerTherapy {  get; set; }

        //public override string ToString() => this.ToStringProperty();


    }
}

using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Volunteer
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Address {  get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public Role Role { get; set; }
        public bool Action { get; set; }
        public double? MaxDistanceReceiveCalling { get; set; }
        public DistanceType DistanceType { get;set; }
        public int TotalCallsHandled { get;set;}
        public int TotalCallsCanceled { get; set; }
        public int TotalCallsChoseHandleHaveExpired { get; set; }
        public BO.CallInProgress? CallingVolunteerTherapy {  get; set; }
       
    }
}

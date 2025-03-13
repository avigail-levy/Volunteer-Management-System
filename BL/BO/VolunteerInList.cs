using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BO
{
    public class VolunteerInList
    {
        public int Id {  get; init; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int TotalCallsHandledByVolunteer { get; set; }//שאילתה
        public int TotalCallsCanceledByVolunteer { get; set; }//שאילתה
        public int TotalExpiredCallingsByVolunteer { get; set; }//שאילתה
        public int? IDCallInHisCare { get; set; }
        public CallType CallType { get; set; } = CallType.None;//?

        //public override string ToString() => this.ToStringProperty();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallAssignInList
    {
        //ישות לצפייה בלבד אין בדיקות תקינות 
        public int? VolunteerId { get; init; }
        public string? Name { get; set; }
        public DateTime EntryTimeForTreatment { get; set; }
        public DateTime? EndOfTreatmentTime {  get; set; }
        public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
         

    }
}

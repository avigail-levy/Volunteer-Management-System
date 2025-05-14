using DalApi;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace Helpers
{
    internal static class VolunteerManager
    {
        internal static ObserverManager Observers = new(); //stage 5
        private class OSMGeocodeResponse
        {
            public string display_name { get; set; }
        }
        private static IDal s_dal = Factory.Get; //stage 4
        /// <summary>
        /// Function to check the correctness of volunteer details
        /// </summary>
        /// <param name="volunteer">Volunteer for testing</param>
        /// <returns>true if correct</returns>
        /// <exception cref="BO.BlInvalidValueException">Invalid volunteer details</exception>
        public static bool IntegrityCheck(BO.Volunteer volunteer)
        {
            if (volunteer.MaxDistanceForCall < 0)
                throw new BO.BlInvalidValueException("max distance for call must be positive");
            //Email health check
            if (!Regex.IsMatch(volunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {

            }
            //Phone check
            if (!Regex.IsMatch(volunteer.Phone, @"^\d{10}$"))
            {
                throw new BO.BlInvalidValueException("Invalid phone number format");
            }
            // ID check
            if (!IsValidIsraeliID(volunteer.Id))
            {
                throw new BO.BlInvalidValueException("Invalid Israeli ID number");
            }

            return true;
        }
        /// <summary>
        /// Checking if the id is valid
        /// </summary>
        /// <param name="id">id to check</param>
        /// <returns> true if the ID is valid, otherwise false</returns>
        static public bool IsValidIsraeliID(int id)
        {

            string idStr = id.ToString().PadLeft(9, '0');
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int num = (idStr[i] - '0') * ((i % 2) + 1);
                sum += num > 9 ? num - 9 : num;
            }
            return sum % 10 == 0;
        }
        /// <summary>
        /// Function to convert degrees to radians
        /// </summary>
        /// <param name="degrees">Degrees to convert: double</param>
        /// <returns>Degrees in radians</returns>
        internal static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        /// <summary>
        /// Function that calculates the longitude and latitude of an address
        /// </summary>
        /// <param name="address">Address for calculation: string</param>
        /// <returns>An array of length 2 where the first index is width and the second index is length</returns>
        public static double[]? CalcCoordinates(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }
            string link = $"https://geocode.maps.co/search?q={Uri.EscapeDataString(address)}&api_key=679a8da6c01a6853187846vomb04142";

            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(link);
                    var result = JsonSerializer.Deserialize<GeocodeResponse[]>(response);
                    if (result == null || result.Length == 0)
                    {
                        throw new BO.BlInvalidValueException("Invalid address.");
                    }
                    double latitude = double.Parse(result[0].lat);
                    double longitude = double.Parse(result[0].lon);
                    return [latitude, longitude];
                }
            }
            catch 
            {
                return null;
            }
        }
        public class GeocodeResponse
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
        /// <summary>
        /// Function to calculate distance between volunteer and call
        /// </summary>
        /// <param name="addressVol">Volunteer address</param>
        /// <param name="addressCall">Call address</param>
        /// <returns>distance</returns>
        /// <exception cref="BO.BlInvalidValueException">Invalid addresses</exception>
        internal static double CalcDistance(string? addressVol,string addressCall)
        {
            if (addressVol == null)
               throw new BO.BlInvalidValueException("the value of volunteer address can not be null");
            double []? volunteerLonLat = CalcCoordinates(addressVol);
            double[]? callLonLat = CalcCoordinates(addressCall);
            if (volunteerLonLat?[0] == null || volunteerLonLat?[1] == null) 
                return 0;
            const double R = 6371; // רדיוס כדור הארץ בק"מ
            double dLat = DegreesToRadians(volunteerLonLat[0] - callLonLat[0]);
            double dLon = DegreesToRadians(volunteerLonLat[1] - callLonLat[1]);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(callLonLat[0])) * Math.Cos(DegreesToRadians(volunteerLonLat[0])) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // המרחק בקילומטרים
        }
        /// <summary>
        /// The assignment that volunteer handles
        /// </summary>
        /// <param name="idVol">id volunteer</param>
        /// <returns>DO.Assignment</returns>
        internal static DO.Assignment? GetCallInTreatment(int idVol)
        {
            var assignVol = s_dal.Assignment.ReadAll(a => a.VolunteerId == idVol);
            DO.Assignment? assignInTreatment = (from a in assignVol
                                                let call = s_dal.Call.Read(a.CallId)
                                                where call != null &&
                                                      (CallManager.GetStatusCall(call) == BO.StatusCall.InTreatment ||
                                                       CallManager.GetStatusCall(call) == BO.StatusCall.InTreatmentAtRisk)
                                                select a).FirstOrDefault();
            return assignInTreatment;
        }
        /// <summary>
        /// Call status in treatment
        /// </summary>
        /// <param name="call">call to get status</param>
        /// <returns>StatusCallInProgress</returns>
        internal static BO.StatusCallInProgress GetCallInProgress(DO.Call call)
        {
            return call.MaxTimeFinishCall - AdminManager.Now > s_dal.Config.RiskRange ?
                BO.StatusCallInProgress.InTreatment : BO.StatusCallInProgress.InRiskTreatment;
        }
        /// <summary>
        /// Count of calls of a specific treatment termination type
        /// </summary>
        /// <param name="type">Type of treatment termination</param>
        /// <param name="assignVol">Volunteer's collection of assignments</param>
        /// <returns></returns>
        internal static int CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination type, IEnumerable<DO.Assignment> assignVol)
        {

            return (from a in assignVol
                    where a.TypeOfTreatmentTermination == type
                    select a).Count();
        }
        /// <summary>
        /// Function that converts BO.Volunteer to DO.Volunteer
        /// </summary>
        /// <param name="volunteer">BO.Volunteer</param>
        /// <param name="role">Volunteer role</param>
        /// <returns>DO.Volunteer</returns>
        internal static DO.Volunteer CreateDoVolunteer(BO.Volunteer volunteer, DO.Role? role = null)
        {
            double[]? latlon = CalcCoordinates(volunteer.Address ?? null);
            IntegrityCheck(volunteer);
            DO.Volunteer doVolunteer = new(
                volunteer.Id,
                volunteer.Name,
                volunteer.Phone,
                volunteer.Email,
                role == null ? (DO.Role)volunteer.Role : (DO.Role)role,
                volunteer.Active,
                (DO.DistanceType)volunteer.DistanceType,
                latlon?[0],
                latlon?[1],
                volunteer.Password,
                latlon==null?null: volunteer.Address,
                volunteer.MaxDistanceForCall
                 );
            return doVolunteer;
        }
    }


}
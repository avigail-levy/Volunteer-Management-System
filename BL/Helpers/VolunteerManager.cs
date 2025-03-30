using DalApi;
using DO;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace Helpers
{
    internal static class VolunteerManager
    {
        
        private class OSMGeocodeResponse
        {
            public string display_name { get; set; }
        }
        private static IDal s_dal = Factory.Get; //stage 4
        public static bool IntegrityCheck(BO.Volunteer volunteer)
        {
            //Email health check
            if (!Regex.IsMatch(volunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                throw new BO.BlInvalidValueException("Invalid email format");
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
            //Address check
            if (!IsValidAddress(volunteer.Latitude, volunteer.Longitude))
            {
                throw new BO.BlInvalidValueException("Address cannot be empty");
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
        /// Checking if the address is valid
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <returns>true if the address is valid, otherwise false</returns>
       
        public static bool IsValidAddress(double? lon, double? lat)
        {
            string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}";

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = client.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));

            if (!response.IsSuccessStatusCode) return false;

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<OSMGeocodeResponse>(jsonResponse);

            return !string.IsNullOrWhiteSpace(result?.display_name);
        }
        internal static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        internal static double CalcDistance(double lat1, double lon1, double? volLat2, double? volLon2)
        {
            if (volLat2 == null || volLon2 == null) return 0;
            const double R = 6371; // רדיוס כדור הארץ בק"מ
            double dLat = DegreesToRadians((double)volLat2 - lat1);
            double dLon = DegreesToRadians((double)volLon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians((double)volLat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // המרחק בקילומטרים
        }
        internal static DO.Assignment GetCallInTreatment(int idVol)
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
        internal static BO.StatusCallInProgress GetCallInProgress(DO.Call call)
        {
            return call.MaxTimeFinishCall - ClockManager.Now > s_dal.Config.RiskRange ?
                BO.StatusCallInProgress.InTreatment : BO.StatusCallInProgress.InRiskTreatment;
        }

        internal static int CountTypeOfTreatmentTermination(TypeOfTreatmentTermination type, IEnumerable<Assignment> assignVol)
        {

            return (from a in assignVol
                    where a.TypeOfTreatmentTermination == type
                    select a).Count();
        }
    }


}

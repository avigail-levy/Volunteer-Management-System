using DalApi;
using System.Net;
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
            //Address check
            if (!IsValidAddress(volunteer.Address))
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

        public static bool IsValidAddress(string? address)
        {
           double [] latlon = CalcCoordinates(address);
            string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latlon[0]}&lon={latlon[1]}";

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="BO.BlInvalidValueException"></exception>
        public static double[]? CalcCoordinates(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }
            string link = $"https://geocode.maps.co/search?q={Uri.EscapeDataString(address)}&api_key=679a8da6c01a6853187846vomb04142";
            Console.WriteLine(link);

            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(link);
                    Console.WriteLine(response);
                    var result = JsonSerializer.Deserialize<GeocodeResponse[]>(response);
                    if (result == null || result.Length == 0)
                    {
                        throw new BO.BlInvalidValueException("Invalid address.");
                    }
                    double latitude = double.Parse(result[0].latitude);
                    double longitude = double.Parse(result[0].longitude);
                    Console.WriteLine($"Latitude: {result[0].latitude}, Longitude: {result[0].longitude}");
                    return [latitude, longitude];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public class GeocodeResponse
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        internal static double CalcDistance(string addressVol,string addressCall)
        {  
            double [] volunteerLonLat = CalcCoordinates(addressVol);
            double[] callLonLat = CalcCoordinates(addressCall);
            if (volunteerLonLat[0] == null || volunteerLonLat[1] == null) return 0;
            const double R = 6371; // רדיוס כדור הארץ בק"מ
            double dLat = DegreesToRadians(volunteerLonLat[0] - callLonLat[0]);
            double dLon = DegreesToRadians(volunteerLonLat[1] - callLonLat[1]);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(callLonLat[0])) * Math.Cos(DegreesToRadians(volunteerLonLat[0])) *
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

        internal static int CountTypeOfTreatmentTermination(DO.TypeOfTreatmentTermination type, IEnumerable<DO.Assignment> assignVol)
        {

            return (from a in assignVol
                    where a.TypeOfTreatmentTermination == type
                    select a).Count();
        }
        internal static DO.Volunteer CreateDoVolunteer(BO.Volunteer volunteer, DO.Role? role = null)
        {
            IntegrityCheck(volunteer);
            DO.Volunteer doVolunteer = new(
                volunteer.Id,
                volunteer.Name,
                volunteer.Phone,
                volunteer.Email,
                role == null ? (DO.Role)volunteer.Role : (DO.Role)role,
                volunteer.Active,
                (DO.DistanceType)volunteer.DistanceType,
                CalcCoordinates(volunteer.Address)[0],
                CalcCoordinates(volunteer.Address)[1],
                volunteer.Password,
                volunteer.Address,
                volunteer.MaxDistanceForCall
                 );
            return doVolunteer;
        }
    }


}
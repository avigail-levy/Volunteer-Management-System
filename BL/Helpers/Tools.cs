using System.Text.Json;
using System.Text.RegularExpressions;

namespace Helpers
{
    static internal class Tools
    {
        private class OSMGeocodeResponse
        {
            public string display_name { get; set; }
        }

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
            //Adress check
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
        /// Checking if the adress is valid
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <returns>true if the adress is valid, otherwise false</returns>
        static public bool IsValidAddress(double? lon, double? lat)
        {
            string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}";

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = client.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));

            if (!response.IsSuccessStatusCode) return false;

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<OSMGeocodeResponse>(jsonResponse);

            return !string.IsNullOrWhiteSpace(result?.display_name);
        }
        //    {

        //        string str = "";
        // foreach (PropertyInfo item in typeof(T).GetProperties())
        // {
        //     var value = item.GetValue(t, null);
        //        str += item.Name + ": ";
        //     if (value is not string && value is IEnumerable)
        //     {
        //         str += "\n";
        //         foreach (var it in (IEnumerable<object>) value)
        //         {
        //             str += it.ToString() + '\n';
        //         }
        //}
        //     else
        //    str += value?.ToString() + '\n';
        //     //str += "\n" + item.CourseName + ": " + item.GetValue(t, null);
        // }
        // return str;
        //}
    }

}
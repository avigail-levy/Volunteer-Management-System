using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Accessories
{
    public class ReadHelper
    {
        public static int ReadInt(string message, int? minValue = null, int? maxValue = null)
        {
            Console.WriteLine(message);
            string numInput = Console.ReadLine();
            int number;
            while (!int.TryParse(numInput, out number)
                &&
                minValue != null ? number >= minValue : true
                &&
                maxValue != null ? number <= minValue : true
                )
            {
                Console.WriteLine("הערך שהקשת אינו תקין. נא הקש שנית!");
                numInput = Console.ReadLine();
            }
            return number;

        }
        public static double ReadDouble(string message)
        {
            Console.WriteLine(message);
            string ageInput = Console.ReadLine();
            double data;
            while (!double.TryParse(ageInput, out data))
            {
                Console.WriteLine("הערך שהקשת אינו תקין. נא הקש שנית!");
                ageInput = Console.ReadLine();
            }
            return data;

        }
        public static string ReadString(string message)
        {
            Console.WriteLine(message);
            string ageInput = Console.ReadLine();
            return ageInput;

        }

        public static DateTime ReadDate(string message)
        {
            Console.WriteLine(message);
            string dateInput = Console.ReadLine();
            DateTime data;
            while (!DateTime.TryParse(dateInput, out data))
            {
                Console.WriteLine("הערך שהקשת אינו תקין. נא הקש שנית!");
                dateInput = Console.ReadLine();
            }
            return data;

        }

        public static T ReadEnum<T>(string message)
        {
            Console.WriteLine(message);
            string enumInput = Console.ReadLine();
            object data;
            while (Enum.TryParse(typeof(T), enumInput, out data))
            {
                Console.WriteLine("הערך שהקשת אינו תקין. נא הקש שנית!");
                enumInput = Console.ReadLine();
            }
            return (T)data;

        }
    }
}
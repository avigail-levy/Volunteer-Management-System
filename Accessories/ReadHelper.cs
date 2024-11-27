using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Accessories
{
    public class ReadHelper
    {
        public static int ReadInt(string message)
        {
            Console.WriteLine(message);
            string numInput = Console.ReadLine();
            int number;
            while (!int.TryParse(numInput, out number))
            {
                Console.WriteLine("The value you entered is invalid. Please enter it again!");
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
                Console.WriteLine("The value you entered is invalid. Please enter it again!");
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
                Console.WriteLine("The value you entered is invalid. Please enter it again!");
                dateInput = Console.ReadLine();
            }
            return data;

        }
        public static bool ReadBool(string message)
        {
            Console.WriteLine(message);
            string dateInput = Console.ReadLine();
            bool data;
            while (!bool.TryParse(dateInput, out data))
            {
                Console.WriteLine("The value you entered is invalid. Please enter it again!");
                dateInput = Console.ReadLine();
            }
            return data;

        }

        public static T ReadEnum<T>(string message)
        {
            Console.WriteLine(message);
            string enumInput = Console.ReadLine();
            object data;
            while (!Enum.TryParse(typeof(T), enumInput, out data))
            {
                Console.WriteLine("The value you entered is invalid. Please enter it again!");
                enumInput = Console.ReadLine();
            }
            return (T)data;

        }
    }
}
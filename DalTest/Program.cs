using Dal;
using DalApi;
using DO;
using System.Runtime.InteropServices;

namespace DalTest
{
    internal class Program
    {

        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1

        private static void Create(string entityName)
        {
            switch (entityName)
            {
                case "Volunteer":
                    CreateVolunteer();
                    break;

                case "Call":
                    CreateCall();
                    break;
                case "Assignment":
                    CreateAssignment();
                    break;
            }
        }
        private static void CreateVolunteer()
        {
            Console.WriteLine("insert id, name, phone, email,role,active,distance type,latitude,longitude,password,address,max distance for call");
            int id = int.Parse(Console.ReadLine());
            string name = Console.ReadLine();
            string phone = Console.ReadLine();
            string email = Console.ReadLine();
            Role role = (Role)int.Parse(Console.ReadLine());
            bool active = bool.Parse(Console.ReadLine());
            DistanceType distanceType = (DistanceType)int.Parse(Console.ReadLine());
            double latitude = double.Parse(Console.ReadLine());
            double longitude = double.Parse(Console.ReadLine());
            string password = Console.ReadLine();
            string address = Console.ReadLine();
            double maxDistanceForCall = double.Parse(Console.ReadLine());
            Volunteer volunteer = new(id, name, phone, email, role, active, distanceType, latitude, longitude, password, address, maxDistanceForCall);
            s_dalVolunteer.Create(volunteer);
        }
        private static void CreateCall()
        {
            Console.WriteLine("insert id, callType, callAddress, latitude,longitude,opening time,call Description,max Time Finish Call");
            int id = int.Parse(Console.ReadLine());
            CallType callType = (CallType)int.Parse(Console.ReadLine());
            string callAddress = Console.ReadLine();
            double latitude = double.Parse(Console.ReadLine());
            double longitude=double.Parse(Console.ReadLine());
            DateTime openingTime = s_dalConfig.Clock;
            string callDescription = Console.ReadLine();
            DateTime? maxTimeFinishCall = s_dalConfig.Clock.AddDays(double.Parse(Console.ReadLine()));
            Call call = new(id, callType, callAddress, latitude, longitude, openingTime, callDescription, maxTimeFinishCall );
            s_dalCall.Create(call);
        }
        private static void CreateAssignment ()
        {
            Console.WriteLine("insert id, call id, volunteer id, entry time for treatment, type of treatment termination,end of treatment time");
            int id = int.Parse(Console.ReadLine());
            int callId = int.Parse(Console.ReadLine());
            int volunteerId = int.Parse(Console.ReadLine());
            DateTime entryTimeForTreatment = s_dalConfig.Clock;
            TypeOfTreatmentTermination typeOfTreatmentTermination = (TypeOfTreatmentTermination)int.Parse(Console.ReadLine());
            DateTime? endOfTreatmentTime = s_dalConfig.Clock.AddDays(double.Parse(Console.ReadLine()));
            Assignment assignment = new(id,callId,volunteerId,entryTimeForTreatment,typeOfTreatmentTermination);
            s_dalAssignment.Create(assignment);


        }

        private static void Delete(string entityName)
        {
            int idToDelete;
            Console.WriteLine("insert id-entity to delete:");
            idToDelete = int.Parse(Console.ReadLine());
            try
            {
                switch (entityName)
                {
                    case "Volunteer":
                        s_dalVolunteer.Delete(idToDelete);
                        break;

                    case "Call":
                        s_dalCall.Delete(idToDelete);
                        break;
                    case "Assignment":
                        s_dalAssignment.Delete(idToDelete);
                        break;
                    default:
                        Console.WriteLine("Unsupported type: " + entityName);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private static void Update(string entityName)
        {
            try
            {
                switch ()
                {
                    default:
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private static void Read(string entityName, int idToRead)
        {
            if (idToRead == 0)
            {
                Console.WriteLine("insert id-entity to delete:");
                idToRead = int.Parse(Console.ReadLine());
            }
            switch (entityName)
            {
                case "Volunteer":
                    if (s_dalVolunteer.Read(idToRead) != null)
                    {
                        Volunteer volunteer = s_dalVolunteer.Read(idToRead);
                        // הצגת המידע של המשימה
                        Console.WriteLine("Volunteer Details:");
                        Console.WriteLine($"Volunteer ID: {volunteer.Id}");
                        Console.WriteLine($"Volunteer name: {volunteer.Name}");
                        Console.WriteLine($"Volunteer phone: {volunteer.Phone}");
                        Console.WriteLine($"Volunteer email: {volunteer.Email}");
                        Console.WriteLine($"Volunteer role: {volunteer.Role}");
                        Console.WriteLine($"Active: {volunteer.Active}");
                        Console.WriteLine($"Distance type: {volunteer.DistanceType}");
                        Console.WriteLine($"Latitude: {volunteer.Latitude?.ToString() ?? "there isn't Latitude"}");
                        Console.WriteLine($"Longitude: {volunteer.Longitude?.ToString() ?? "there isn't Longitude"}");
                        Console.WriteLine($"Password: {volunteer.Password?.ToString() ?? "there isn't Password"}");
                        Console.WriteLine($"Address: {volunteer.Address?.ToString() ?? "there isn't Address"}");
                        Console.WriteLine($"Max distance for call: {volunteer.MaxDistanceForCall?.ToString() ?? "there isn't max distance for call"}");
                    }
                    break;
                case "Call":
                    if (s_dalCall.Read(idToRead) != null)
                    {
                        Call call = s_dalCall.Read(idToRead);
                        // הצגת המידע של המשימה
                        Console.WriteLine("Call Details:");
                        Console.WriteLine($"Call ID: {call.Id}");
                        Console.WriteLine($"Call type: {call.CallType}");
                        Console.WriteLine($"Call address: {call.CallAddress}");
                        Console.WriteLine($"Latitude: {call.Latitude}");
                        Console.WriteLine($"Longitude: {call.Longitude}");
                        Console.WriteLine($"Opening time: {call.OpeningTime.ToString()}");
                        Console.WriteLine($"Call description: {call.CallDescription?.ToString() ?? "there isn't call description"}");
                        Console.WriteLine($"Max time finish call: {call.MaxTimeFinishCall?.ToString() ?? "there isn't max time finish to call"}");
                    }
                    break;

                case "Assignment":
                    if (s_dalAssignment.Read(idToRead) != null)
                    {
                        Assignment assignment = s_dalAssignment.Read(idToRead);
                        // הצגת המידע של המשימה
                        Console.WriteLine("Assignment Details:");
                        Console.WriteLine($"Assignment ID: {assignment.Id}");
                        Console.WriteLine($"Call ID: {assignment.CallId}");
                        Console.WriteLine($"Volunteer ID: {assignment.VolunteerId}");
                        Console.WriteLine($"Entry Time of Treatment: {assignment.EntryTimeForTreatment}");
                        Console.WriteLine($"End Time of Treatment: {assignment.EndOfTreatmentTime?.ToString() ?? "N/A"}");
                        Console.WriteLine($"Type of End Treatment: {assignment.TypeOfTreatmentTermination?.ToString() ?? "N/A"}");
                    }
                    break;

                default:
                    Console.WriteLine("Unsupported type: " + entityName);
                    break;
            }

        }
        private static void ReadAll(string entityName)
        {
            switch (entityName)
            {
                case "Volunteer":
                    foreach (var volunteer in s_dalVolunteer.ReadAll())
                        Read("Volunteer", volunteer.Id);
                    break;
                case "Call":
                    foreach (var call in s_dalCall.ReadAll())
                        Read("Call", call.Id);
                    break;
                case "Assignment":
                    foreach (var assignment in s_dalAssignment.ReadAll())
                        Read("Assignment", assignment.Id);
                    break;
                default:
                    break;
            }
        }
        private static void DeleteAll(string entityName)
        {
            switch (entityName)
            {
                case "Volunteer":
                    s_dalVolunteer.DeleteAll();
                    break;
                case "Call":
                    s_dalCall.DeleteAll();
                    break;
                case "Assignment":
                    s_dalAssignment.DeleteAll();
                    break;
                default:
                    Console.WriteLine("Unsupported type: " + entityName);
                    break;
            }
        }

        static void PrintMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            foreach (var option in Enum.GetValues<MainMenuOptions>())
            {
                Console.WriteLine($"{(int)option}: {option}");
            }
            Console.Write("Choose an option: ");
        }
        static void PrintCrudMenu()
        {
            Console.WriteLine("\nMain Menu:");
            foreach (var option in Enum.GetValues<CrudMenuOptions>())
            {
                Console.WriteLine($"{(int)option}: {option}");
            }
            Console.Write("Choose an option: ");
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="args"></param>

        static void Main(string[] args)
        {
            while (true)
            {
                //פונקציה שמדפסיסה את התפריט הראשי
                PrintMainMenu();
                string input = Console.ReadLine();
                int numericChoice;

                // בדיקת קלט
                if (!int.TryParse(input, out numericChoice) || !Enum.IsDefined(typeof(MainMenuOptions), numericChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                // המרה לערך Enum של MainMenuOptions
                MainMenuOptions choice = (MainMenuOptions)numericChoice;

                switch (choice)
                {
                    case MainMenuOptions.Exit:
                        return;
                    case MainMenuOptions.VolunteerMenu:
                        CrudMenu("Volunteer");
                        break;
                    case MainMenuOptions.AssignmentMenu:
                        CrudMenu("Assignment");
                        break;
                    case MainMenuOptions.CallMenu:
                        CrudMenu("Call");
                        break;
                    case MainMenuOptions.InitializeDatabase:
                        Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                        break;
                    case MainMenuOptions.ResetDatabase:
                        s_dalConfig.Reset();
                        break;
                }
            }
        }
        static void CrudMenu(string entityName)
        {
            PrintCrudMenu();
            string input = Console.ReadLine();
            int numericCrudChoice;

            // בדיקת קלט
            if (!int.TryParse(input, out numericCrudChoice) || !Enum.IsDefined(typeof(CrudMenuOptions), numericCrudChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }
            // המרה לערך Enum של MainMenuOptions
            CrudMenuOptions choice = (CrudMenuOptions)numericCrudChoice;

            switch (choice)
            {

                case CrudMenuOptions.Exit:
                    return;

                case CrudMenuOptions.Create:
                    Create(entityName);
                    break;
                case CrudMenuOptions.Read:
                    Read(entityName, 0);
                    break;
                case CrudMenuOptions.ReadAll:
                    ReadAll(entityName);
                    break;
                case CrudMenuOptions.Update:
                    Update(entityName);
                    break;
                case CrudMenuOptions.Delete:
                    Delete(entityName);
                    break;
                case CrudMenuOptions.DeleteAll:
                    DeleteAll(entityName);
                    break;
                default:
                    break;
            }
        }
    }
}

using Dal;
using DalApi;
using DO;
using Accessories;
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
            Volunteer volunteer = new Volunteer()
            {
                Id = ReadHelper.ReadInt("insert id volunteer: ", 200000000, 400000000),
                Name = ReadHelper.ReadString("insert full name: "),
                Phone = ReadHelper.ReadString("insert phone number: "),
                Email = ReadHelper.ReadString("insert email: "),
                Role = ReadHelper.ReadEnum<Role>("insert role "),
                Active = ReadHelper.ReadBool("insert is active: "),
                DistanceType = ReadHelper.ReadEnum<DistanceType>("insert distance type "),
                Latitude = ReadHelper.ReadDouble("insert latitude "),
                Longitude = ReadHelper.ReadDouble("insert longitude "),
                Password = ReadHelper.ReadString("insert password: "),
                Address = ReadHelper.ReadString("insert address: "),
                MaxDistanceForCall = ReadHelper.ReadDouble("insert max distance for call")
            };
            s_dalVolunteer.Create(volunteer);
        }
        private static void CreateCall()
        {
            Call call = new Call()
            {
                CallType = ReadHelper.ReadEnum<CallType>("insert call type: "),
                CallAddress = ReadHelper.ReadString("insert call address: "),
                Latitude = ReadHelper.ReadDouble("insert a latitude:"),
                Longitude = ReadHelper.ReadDouble("insert a longitude:"),
                OpeningTime = ReadHelper.ReadDate("insert opening time for call: "),
                CallDescription = ReadHelper.ReadString("insert call description: "),
                MaxTimeFinishCall = ReadHelper.ReadDate("insert max time finish call:")
            };
            s_dalCall.Create(call);
        }
        private static void CreateAssignment()
        {
            Assignment assignment = new Assignment()
            {
                CallId = ReadHelper.ReadInt("insert id of call: "),
                VolunteerId = ReadHelper.ReadInt("insert id of volunteer: "),
                EntryTimeForTreatment = ReadHelper.ReadDate("insert entry time for treatment: "),
                TypeOfTreatmentTermination = ReadHelper.ReadEnum<TypeOfTreatmentTermination>("insert type of treatment termination"),
                EndOfTreatmentTime = ReadHelper.ReadDate("insert end of treatment time")
            };
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
        private static void UpdateCall()
        {
            Console.WriteLine("insert id-entity to update:");
            int idToUpdate = int.Parse(Console.ReadLine());
            try
            {
                Call oldCall = s_dalCall.Read(idToUpdate);
                Console.WriteLine("Enter the data to create a new object of type call:");
                Console.WriteLine("Enter the data of: type of call, full address, latitude, longitude, opening time, maximum time of finish call, description");
                Call newCall = new Call()
                {
                    CallType = int.TryParse(Console.ReadLine(), out int typeOfCall) ? (CallType)typeOfCall : oldCall.CallType,
                    CallAddress = Console.ReadLine() ?? oldCall.CallAddress,
                    Latitude = double.TryParse(Console.ReadLine(), out double latitude) ? latitude : oldCall.Latitude,
                    Longitude = double.TryParse(Console.ReadLine(), out double Longitude) ? Longitude : oldCall.Longitude,
                    OpeningTime = DateTime.TryParse(Console.ReadLine(), out DateTime OpeningTime) ? OpeningTime : oldCall.OpeningTime,
                    MaxTimeFinishCall = DateTime.TryParse(Console.ReadLine(), out DateTime MaximumTimeFinishCall) ? MaximumTimeFinishCall : oldCall.MaxTimeFinishCall
                };
                s_dalCall.Update(newCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void UpdateVolunteer()
        {
            Console.WriteLine("insert id-entity to update:");
            int idToUpdate = int.Parse(Console.ReadLine());
            try
            {
                Volunteer oldVolunteer = s_dalVolunteer.Read(idToUpdate);
                Console.WriteLine("Enter the data to create a new object of type volunteer:");
                Console.WriteLine("Enter the data of:  full name, phone, email, role, active, distance type,latitude,longitude,password, address, max distance for call");
                Volunteer newVolunteer = new Volunteer()
                {
                    Id = oldVolunteer.Id,
                    Name = Console.ReadLine() ?? oldVolunteer.Name,
                    Phone = Console.ReadLine() ?? oldVolunteer.Phone,
                    Email = Console.ReadLine() ?? oldVolunteer.Email,
                    Role = int.TryParse(Console.ReadLine(), out int role) ? (Role)role : oldVolunteer.Role,
                    Active = bool.TryParse(Console.ReadLine(), out bool active) ? active : oldVolunteer.Active,
                    DistanceType = int.TryParse(Console.ReadLine(), out int distanceType) ? (DistanceType)distanceType : oldVolunteer.DistanceType,
                    Latitude = double.TryParse(Console.ReadLine(), out double latitude) ? latitude : oldVolunteer.Latitude,
                    Longitude = double.TryParse(Console.ReadLine(), out double Longitude) ? Longitude : oldVolunteer.Longitude,
                    Password = Console.ReadLine() ?? oldVolunteer.Password,
                    Address = Console.ReadLine() ?? oldVolunteer.Address,
                    MaxDistanceForCall = double.TryParse(Console.ReadLine(), out double maxDistanceForCall) ? maxDistanceForCall : oldVolunteer.MaxDistanceForCall,
                };
                s_dalVolunteer.Update(newVolunteer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void UpdateAssignment()
        {
            Console.WriteLine("insert id-entity to update:");
            int idToUpdate = int.Parse(Console.ReadLine());
            try
            {
                Assignment oldAssignment = s_dalAssignment.Read(idToUpdate);
                Console.WriteLine("Enter the data to create a new object of type assignment:");
                Console.WriteLine("insert  call id, volunteer id, entry time for treatment, type of treatment termination,end of treatment time");
                Assignment newAssignment = new Assignment()
                {
                    CallId = int.TryParse(Console.ReadLine(), out int callid) ? callid : oldAssignment.CallId,
                    VolunteerId = int.TryParse(Console.ReadLine(), out int volunteerId) ? volunteerId : oldAssignment.VolunteerId,
                    EntryTimeForTreatment = DateTime.TryParse(Console.ReadLine(), out DateTime entryTimeForTreatment) ? entryTimeForTreatment : oldAssignment.EntryTimeForTreatment,
                    TypeOfTreatmentTermination = int.TryParse(Console.ReadLine(), out int typeOfTreatmentTermination) ? (TypeOfTreatmentTermination)typeOfTreatmentTermination : oldAssignment.TypeOfTreatmentTermination,
                    EndOfTreatmentTime = DateTime.TryParse(Console.ReadLine(), out DateTime endOfTreatmentTime) ? endOfTreatmentTime : oldAssignment.EndOfTreatmentTime,
                };
                s_dalAssignment.Update(newAssignment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void Update(string entityName)
        {
            Console.WriteLine("insert id-entity to update:");
            int idToUpdate = int.Parse(Console.ReadLine());
            try
            {
                switch (entityName)
                {
                    case "Volunteer":
                        UpdateVolunteer();
                        break;
                    case "Call":
                        UpdateCall();
                        break;
                    case "Assignment":
                        UpdateAssignment();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private static void ReadVolunteer(int idToRead)
        {
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
        }
        private static void ReadCall(int idToRead)
        {
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
        }
        private static void ReadAssignment(int idToRead)
        {
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
                    ReadVolunteer(idToRead);
                    break;
                case "Call":
                    ReadCall(idToRead);
                    break;

                case "Assignment":
                    ReadAssignment(idToRead);
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
        static void MenuConfig()
        {
            Console.Write("Choose an option: ");
            int numericChoice = int.Parse(Console.ReadLine());
            ConfigMenuOptions choice = (ConfigMenuOptions)numericChoice;
            switch (choice)
            {

                case ConfigMenuOptions.Exit:
                    return;
                case ConfigMenuOptions.AdvanceClockMinute:
                    s_dalConfig.Clock.AddMinutes(1);
                    break;
                case ConfigMenuOptions.AdvanceClockHour:
                    s_dalConfig.Clock.AddHours(1);
                    break;
                case ConfigMenuOptions.ViewCurrentClock:
                    Console.WriteLine(s_dalConfig.Clock);
                    break;
                case ConfigMenuOptions.NewConfigValue:
                    s_dalConfig.Clock = s_dalConfig.Clock.AddMinutes(3);
                    break;
                case ConfigMenuOptions.ViewConfigValue:
                    Console.WriteLine($"the risk range is: {s_dalConfig.RiskRange.ToString()}");
                    break;
                case ConfigMenuOptions.ResetConfigValues:
                    s_dalConfig.Reset();
                    break;
                default:
                    break;
            }
        }
        static void ShowAll()
        {
            ReadAll("Volunteer");
            ReadAll("Call");
            ReadAll("Assignment");
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
                    case MainMenuOptions.ConfigMenu:
                        MenuConfig();
                        break;
                    case MainMenuOptions.ShowAll:
                        ShowAll();
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

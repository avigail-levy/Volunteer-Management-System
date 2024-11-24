using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {

        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1

        private static void Delete<T>()
        {
            int idToDelete;
            Console.WriteLine("insert id-entity to delete:");
            idToDelete=int.Parse(Console.ReadLine());
            try
            {
                switch (T)
                {
                    case Volunteer:
                        s_dalVolunteer.Delete(idToDelete);
                        break;
                    case Call:
                        s_dalCall.Delete(idToDelete);
                        break;
                    case Assignment:
                        s_dalAssignment.Delete(idToDelete);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static void Update<T>()
        {

        }
        private static void Read<T>()
        {

        }
        private static void ReadAll<T>()
        {

        }
        private static void DeleteAll<T>()
        {

        }
        private static void ChooseFunction<T>()
        {
            string choose_function;
            Console.WriteLine("Insert your function's choice: Add/Delete/Update/Read/ReadAll/DeleteAll");
            choose_function = Console.ReadLine();
            switch (choose_function)
            {
                case "Add":
                    ChooseFunction<Volunteer>();
                    break;
                case "Delete":
                    Delete<Call>();
                    break;
                case "Update":
                    ChooseFunction<Assignment>();
                    break;
                case "Read":
                    ChooseFunction<Assignment>();
                    break;
                case "ReadAll":
                    ChooseFunction<Assignment>();
                    break;
                case "DeleteAll":
                    ChooseFunction<Assignment>();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
        static void Main(string[] args)
        {
            string choose_entity;
            Console.WriteLine("Insert your entity's choice: Volunteer/Call/Assignment");
            choose_entity = Console.ReadLine();

            switch (choose_entity)
            {
                case "Volunteer":
                    ChooseFunction<Volunteer>();
                    break;
                case "Call":
                    ChooseFunction<Call>();
                    break;
                case "Assignment":
                    ChooseFunction<Assignment>();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}

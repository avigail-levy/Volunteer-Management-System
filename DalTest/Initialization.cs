namespace DalTest;
using DalApi;
using DO;
using Microsoft.VisualBasic;
using System;
using System.Net;
using System.Net.Mail;

public static class Initialization
{
    private static IVolunteer? s_dalVolunteer; //stage 1
    private static ICall? s_dalCall; //stage 1
    private static IAssignment? s_dalAssignment; //stage 1
    private static IConfig? s_dalConfig; //stage 1
    private static readonly Random s_rand = new();
    const int MIN_ID = 200000000;
    const int MAX_ID = 400000000;
    private static void createVolunteer()
    {
        string[] valunteerNames =
        { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof",
          "David Gold", "Rachel Green", "Miriam Azulay", "Eyal Shani", "Noa Bar", "Yael Harel" };
        string[] phones = { "0504133382", "0556726282", "0527175821", "0527175820", "0504160838", "0504156891",
        "0503133382", "0556724282", "0527175221", "0528175820", "0504160837", "0504756891",
        "0504113382", "0556723282", "0527575821", "0527175840", "0504160828", "0504106891",
        "0504133383", "0526726282", "0527175821", "0527145820", "0504166838", "0504196891"};
        for (int i = 0; i < valunteerNames.Length; i++)
        {
            int id;
            string phone;
            string name;
            string email;
            Role role;
            bool active;
            DistanceType distanceType;
            double? latitude;
            double? longitude;
            string? password;
            string? address;
            double? maxDistanceForCall;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);
            phone = phones[i];
            name = valunteerNames[i];
            email = name + "@gmail.com";
            role = i % 2 == 0 ? Role.Volunteer : Role.Manager;
            active = i % 2 == 0 ? true : false;
            distanceType = (DistanceType)s_rand.Next(0, 3);
            latitude = s_rand.NextDouble() * 360 - 180;
            longitude = s_rand.NextDouble() * 360 - 180;
            password = name + phone;
            address = $"{s_rand.Next(1, 500)} {new[] { "Main", "Maple", "Elm", "Oak", "Cedar" }[s_rand.Next(5)]} St";
            maxDistanceForCall = s_rand.NextDouble() * 50;
            s_dalVolunteer!.Create(new(id, name, phone, email, role, active, distanceType, latitude, longitude, password, address));
        }
    }

    private static void createCall()
    {
        DO.CallType callType;
        string callAddress;
        double latitude;
        double longitude;
        DateTime openingTime;
        string? callDescription;
        DateTime? maxTimeFinishCall;
        for (int i = 0; i < 50; i++)
        {

            callType = (DO.CallType)s_rand.Next(0, 6);
            callAddress = $"{s_rand.Next(1, 500)} {new[] { "Aharonson", "Shalos Hashaot", "Rabi Akiva", "Oak", "Cedar" }[s_rand.Next(5)]} St";
            latitude = s_rand.NextDouble() * 360 - 180; ;
            longitude = s_rand.NextDouble() * 360 - 180; ;
            openingTime = s_dalConfig.Clock;
            callDescription = callAddress + " " + latitude.ToString() + " " + longitude.ToString() + " " + openingTime.ToString();
            maxTimeFinishCall = openingTime.AddDays(5);
            s_dalCall!.Create(new(0, callType, callAddress, latitude, longitude, openingTime, callDescription, maxTimeFinishCall));
        }
    }
    private static void CreateAssignment()
    {/*סימן שאלה ענק??????????????????????????????????????????????????????
      */
        foreach (var i in Enumerable.Range(0, 16))
        {
            // קבלת רשימת המתנדבים
            var volunteers = s_dalVolunteer.ReadAll();
            // חילוץ volunteerId מתוך המתנדב שנמצא באינדקס i
            int volunteerId = volunteers[i].Id;  // גישה לאובייקט המתנדב לפי אינדקס והחזרת ה-Id שלו

            var calls = s_dalCall.ReadAll();
            int callId = calls[i].Id;

            DateTime start = new DateTime(s_dalConfig.Clock.Year - 2, 1, 1); // שנתיים אחורה
            int range = (s_dalConfig.Clock - start).Days;
            DateTime entryTimeOfTreatment = start.AddDays(s_rand.Next(range));
            DateTime? endTimeOfTreatment = entryTimeOfTreatment.AddDays(s_rand.Next(1, 7)); // עד 7 ימים לאחר מכן

            TypeOfTreatmentTermination typeOfEndTreatment = (TypeOfTreatmentTermination)s_rand.Next(0, Enum.GetValues(typeof(TypeOfTreatmentTermination)).Length);

            // יצירת אובייקט מתנדב
            var assignment = new Assignment(
                0,
                callId,
                volunteerId,
                entryTimeOfTreatment,
                typeOfEndTreatment,
                endTimeOfTreatment
            );
            // שמירה למערכת הנתונים
            s_dalAssignment!.Create(assignment);
        }
    }
    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig) //stage 1
    {
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1

        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset(); //stage 1
        s_dalVolunteer.DeleteAll(); //stage 1
        s_dalCall.DeleteAll();
        s_dalAssignment.DeleteAll();
        Console.WriteLine("Initializing Students list ...");
        createVolunteer();
        CreateAssignment();
        createCall();
    }


}


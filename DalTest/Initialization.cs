namespace DalTest;
using DalApi;
using DO;
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
       { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof" };
        string[] phones = { "0504133382", "0556726282", "0527175821", "0527175820", "0504160838", "0504156891" };
        foreach (var name in valunteerNames)
        {
            int id;
            string phone;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
                phone=
            while (s_dalVolunteer!.Read(id) != null);
            //DateTime start = new DateTime(1995, 1, 1);
            //DateTime bdt = start.AddDays(s_rand.Next((s_dalConfig.Clock - start).Days));

            s_dalVolunteer!.Create(new(id, name));
        }
    }
    private static void createCall()
    { }
    private static void createAssignment()
    { }


}


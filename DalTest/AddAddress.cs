using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalTest
{        //מחלקה ליצירת אובייקטים של כתובות 
     internal class AddAddress
    {
        public string? StringAddress;
        public double Latitude;
        public double Longitude;
    
    
        public static List<AddAddress> GetAddAddresses()
        {
            return new List<AddAddress>()
            {
                                new AddAddress() {StringAddress="Herzl Street 1, Tel Aviv", Latitude=32.070446, Longitude=34.794667},
                new AddAddress() {StringAddress="Rothschild Boulevard 16, Tel Aviv", Latitude=32.062819, Longitude=34.774061},
                new AddAddress() {StringAddress="Ben Gurion Street 22, Ramat Gan", Latitude=32.084027, Longitude=34.812335},
                new AddAddress() {StringAddress="Jaffa Street 31, Jerusalem", Latitude=31.785704, Longitude=35.211248},
                new AddAddress() {StringAddress="Haifa Road 101, Haifa", Latitude=32.811834, Longitude=34.983419},
                new AddAddress() {StringAddress="Weizmann Street 19, Kfar Saba", Latitude=32.174547, Longitude=34.905562},
                new AddAddress() {StringAddress="HaNasi Boulevard 15, Haifa", Latitude=32.802460, Longitude=34.985614},
                new AddAddress() {StringAddress="Herzl Street 77, Rishon Lezion", Latitude=31.965539, Longitude=34.803267},
                new AddAddress() {StringAddress="Begin Road 48, Tel Aviv", Latitude=32.069509, Longitude=34.783370},
                new AddAddress() {StringAddress="Kaplan Street 12, Tel Aviv", Latitude=32.073151, Longitude=34.791167},
                new AddAddress() {StringAddress="HaPalmach Street 5, Be'er Sheva", Latitude=31.251973, Longitude=34.791462},
                new AddAddress() {StringAddress="King George Street 27, Tel Aviv", Latitude=32.073187, Longitude=34.775301},
                new AddAddress() {StringAddress="Sderot Yerushalayim 45, Jaffa", Latitude=32.051958, Longitude=34.758285},
                new AddAddress() {StringAddress="Hertzel Street 60, Hadera", Latitude=32.434046, Longitude=34.918386},
                new AddAddress() {StringAddress="Allenby Street 99, Tel Aviv", Latitude=32.068588, Longitude=34.769863},
                new AddAddress() {StringAddress="HaNassi Street 2, Rehovot", Latitude=31.896828, Longitude=34.812281},
                new AddAddress() {StringAddress="HaBanim Street 8, Tiberias", Latitude=32.794046, Longitude=35.531128},
                new AddAddress() {StringAddress="Balfour Street 14, Bat Yam", Latitude=32.019664, Longitude=34.745525},
                new AddAddress() {StringAddress="Jabotinsky Street 33, Petah Tikva", Latitude=32.090468, Longitude=34.878975},
                new AddAddress() {StringAddress="HaGalil Street 12, Safed", Latitude=32.965678, Longitude=35.494012},
                new AddAddress() {StringAddress="Herzl Street 8, Netanya", Latitude=32.332015, Longitude=34.855212},
                new AddAddress() {StringAddress="Bar Kochva Street 5, Ramat Gan", Latitude=32.083171, Longitude=34.814524},
                new AddAddress() {StringAddress="Rambam Street 22, Eilat", Latitude=29.557669, Longitude=34.951925},
                new AddAddress() {StringAddress="Ben Yehuda Street 34, Tel Aviv", Latitude=32.081943, Longitude=34.768878},
                new AddAddress() {StringAddress="Hashalom Road 1, Herzliya", Latitude=32.165875, Longitude=34.840357},
                new AddAddress() {StringAddress="Shlomo HaMelekh Street 19, Ashdod", Latitude=31.803685, Longitude=34.641451},
                new AddAddress() {StringAddress="Bar Ilan Street 10, Holon", Latitude=32.013330, Longitude=34.776073},
                new AddAddress() {StringAddress="Derech HaAtsmaut 67, Haifa", Latitude=32.818271, Longitude=35.001148},
                new AddAddress() {StringAddress="Hanasi Street 44, Kiryat Motzkin", Latitude=32.831499, Longitude=35.078012},
                new AddAddress() {StringAddress="Har Hatzofim Street 3, Jerusalem", Latitude=31.793755, Longitude=35.241553},
                new AddAddress() {StringAddress="HaTikva Street 7, Ashkelon", Latitude=31.670788, Longitude=34.571228},
                new AddAddress() {StringAddress="Raoul Wallenberg Street 24, Tel Aviv", Latitude=32.111427, Longitude=34.840487},
                new AddAddress() {StringAddress="David Remez Street 2, Beit Shemesh", Latitude=31.746913, Longitude=34.986564},
                new AddAddress() {StringAddress="Rothschild Street 18, Rishon Lezion", Latitude=31.960252, Longitude=34.804289},
                new AddAddress() {StringAddress="Kibbutz Galuyot 23, Tel Aviv", Latitude=32.045431, Longitude=34.764147},
                new AddAddress() {StringAddress="Bialik Street 10, Ra'anana", Latitude=32.184289, Longitude=34.873586},
                new AddAddress() {StringAddress="Hertzel Boulevard 33, Haifa", Latitude=32.815532, Longitude=34.995479},
                new AddAddress() {StringAddress="Dizengoff Street 65, Tel Aviv", Latitude=32.080126, Longitude=34.774455},
                new AddAddress() {StringAddress="Arlozorov Street 21, Netanya", Latitude=32.329859, Longitude=34.855738},
                new AddAddress() {StringAddress="Shapira Street 2, Kiryat Shmona", Latitude=33.208588, Longitude=35.570471},
                new AddAddress() {StringAddress="Hashomer Street 10, Tel Aviv", Latitude=32.068281, Longitude=34.770236},
                new AddAddress() {StringAddress="Yigal Alon Street 98, Tel Aviv", Latitude=32.069810, Longitude=34.794895},
                new AddAddress() {StringAddress="Henrietta Szold Street 3, Beersheba", Latitude=31.254938, Longitude=34.799763},
                new AddAddress() {StringAddress="Rabin Boulevard 18, Modi'in", Latitude=31.906328, Longitude=35.004601},
                new AddAddress() {StringAddress="HaMoshava Street 7, Kfar Tavor", Latitude=32.687209, Longitude=35.420792},
                new AddAddress() {StringAddress="Emek Refaim Street 34, Jerusalem", Latitude=31.764492, Longitude=35.220554},
                new AddAddress() {StringAddress="Ben Gurion Boulevard 25, Bat Yam", Latitude=32.017102, Longitude=34.747173},
                new AddAddress() {StringAddress="Hertzel Street 9, Nahariya", Latitude=33.004863, Longitude=35.092421},
                new AddAddress() {StringAddress="Palmach Street 15, Kiryat Gat", Latitude=31.607161, Longitude=34.768885}
            };
        }
    }
}

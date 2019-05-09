using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SadovodClasses;
using Xamarin.Essentials;

namespace SadovodMobile
{
    public static class Utilities
    {
        public static void ShowMessage(object sender, string message)
        {
            View view = (View)sender;
            Snackbar
                .Make(view, message, Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public static bool IsFromLatinOrNums(string login)
        {
            if (login == null || login.Length == 0)
            {
                return false;
            }
            foreach (char s in login)
            {
                if (IsNumber(s) || IsLatin(s))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        public static string GetSteadsNotificationText()
        {
            string token = Preferences.Get("token", null);
            var steads = new List<Stead>();
            if (token != null)
            {
                //если есть токен, проверяю его валидность через getlogin
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = client.GetAsync("/api/signup/getlogin").Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpResponseMessage response1 = client.GetAsync("/api/database/DatabaseGetByGardenerID").Result;
                    string mySteads = response1.Content.ReadAsStringAsync().Result;
                    var a = JsonConvert.DeserializeObject<List<DatabaseStead>>(mySteads);
                    foreach (var b in a)
                    {
                        steads.Add(b.Stead);
                    }
                }
                else
                {
                    steads = UserSingleton.Instance.ReadSteadsAsync().Result;
                }
            }
            else
            {
                steads = UserSingleton.Instance.ReadSteadsAsync().Result;
            }
            var result = new StringBuilder();
            foreach (var stead in steads)
            {
                var curSteadInfo = new StringBuilder();
                foreach (var gardenBed in stead.GardenBeds)
                {
                    if (Math.Abs(gardenBed.WaterDate.DayOfYear - DateTime.Now.DayOfYear) >= gardenBed.WaterPeriod)
                    {
                        curSteadInfo.Append($"Полить {gardenBed.Plant.TypeName} {gardenBed.Plant.SortName}\n");
                    }
                    if (Math.Abs(gardenBed.WeedDate.DayOfYear - DateTime.Now.DayOfYear) >= gardenBed.WeedPeriod)
                    {
                        curSteadInfo.Append($"Прополоть {gardenBed.Plant.TypeName} {gardenBed.Plant.SortName}\n");
                    }
                    if (Math.Abs(gardenBed.FertilizeDate.DayOfYear - DateTime.Now.DayOfYear) >= gardenBed.FertilizePeriod)
                    {
                        curSteadInfo.Append($"Удобрить {gardenBed.Plant.TypeName} {gardenBed.Plant.SortName}\n");
                    }
                    if (Math.Abs(gardenBed.PileUpDate.DayOfYear - DateTime.Now.DayOfYear) >= gardenBed.PileUpPeriod)
                    {
                        curSteadInfo.Append($"Полить {gardenBed.Plant.TypeName} {gardenBed.Plant.SortName}\n");
                    }
                }
                if (curSteadInfo.Length==0)
                {
                    continue;
                }
                result.Append($"Для {stead.Name}:\n{curSteadInfo.ToString()}");
            }
            return result.ToString();
        }
        public static string Parse(string stead)
        {
            var g = stead.Split(',');
            var r = new StringBuilder();
            var l = stead.Where(v => v != '\\').ToList();
            var flag = false;
            for (var i = 0; i < l.Count(); i++)
            {
                if (l[i] == '{')
                {
                    flag = true;


                }
                if (flag)
                {
                    r.Append(l[i]);
                    if (l[i] == '}')
                        break;
                }
            }
            return r.ToString();
        }
        public static List<string> GetMagic(string input)
        {
            List<string> myString = new List<string>();
            int cur = 0;
            int start = -1;
            int count = 0;
            while (cur < input.Length)
            {
                if (input[cur] == '{')
                {
                    start = cur;
                    count++;
                }
                if (input[cur] == '}')
                {
                    count--;
                    if (count == 0)
                    {
                        myString.Add(input.Substring(start, cur - start + 1));
                    }
                }
                cur++;
            }
            return myString;
        }

        public static bool IsNumber(char symb)
        {
            return symb >= '0' && symb <= '9';
        }

        public static bool IsLatin(char symb)
        {
            return symb >= 'a' && symb <= 'z' || symb >= 'A' && symb <= 'Z';
        }

        public static DateTime DateTimeFormat(string input)
        {
            try
            {
                int day = int.Parse(input.Substring(0, 2));
                int month = int.Parse(input.Substring(3, 2));
                int year = int.Parse(input.Substring(6, 4));
                int hour = int.Parse(input.Substring(11, 2));
                int minute = int.Parse(input.Substring(14, 2));
                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static bool CheckActionNeed(DateTime last, DateTime now, int period)
        {
            var delta = (now.Date - last.Date).TotalDays;
            return delta >= period;
        }
    }
}
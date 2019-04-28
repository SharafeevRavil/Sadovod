using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

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
    }
}
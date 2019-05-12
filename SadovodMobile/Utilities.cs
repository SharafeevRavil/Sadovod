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
using SkiaSharp;

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

        public static SKPoint ToRealCoords(float inputX, float inputY, SKMatrix matrix)
        {
            SKMatrix inverse;
            matrix.TryInvert(out inverse);
            var ans = ToMatrixCoords(inputX, inputY, inverse);
            return new SKPoint(ans.X, ans.Y);
        }

        public static SKPoint ToRealCoords(SKPoint input, SKMatrix matrix)
        {
            return ToRealCoords(input.X, input.Y, matrix);
        }

        public static SKPoint ToMatrixCoords(float inputX, float inputY, SKMatrix matrix)
        {
            float x = matrix.ScaleX * inputX + matrix.SkewX * inputY + matrix.TransX;
            float y = matrix.SkewY * inputX + matrix.ScaleY * inputY + matrix.TransY;
            return new SKPoint(x, y);
        }

        public static SKPoint ToMatrixCoords(SKPoint input, SKMatrix matrix)
        {
            return ToMatrixCoords(input.X, input.Y, matrix);
        }
    }
}
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
            if(login == null || login.Length == 0)
            {
                return false;
            }
            foreach (char s in login)
            {
                if (s >= 'a' && s <= 'z')
                {
                    continue;
                }
                if (s >= 'A' && s <= 'Z')
                {
                    continue;
                }
                if (s >= '0' && s <= '9')
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
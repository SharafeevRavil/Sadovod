using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Настройки", Theme = "@style/AppTheme.NoActionBar")]
    class SettingsActivity : AppCompatActivity
    {
        EditText lonText;
        EditText latText;
        EditText morningNotifyTime;
        EditText eveningNotifyTime;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsLayout);
            lonText = (EditText)FindViewById(Resource.Id.editText1);
            if (Preferences.ContainsKey("lon"))
            {
                lonText.SetText($"{Preferences.Get("lon", 0.0)}", TextView.BufferType.Normal);
            }
            latText = (EditText)FindViewById(Resource.Id.editText2);
            if (Preferences.ContainsKey("lat"))
            {
                latText.SetText($"{Preferences.Get("lat", 0.0)}", TextView.BufferType.Normal);
            }
            morningNotifyTime = (EditText)FindViewById(Resource.Id.editText3);
            if (Preferences.ContainsKey("morning"))
            {
                morningNotifyTime.SetText($"{Preferences.Get("morning", 6)}", TextView.BufferType.Normal);
            }
            eveningNotifyTime = (EditText)FindViewById(Resource.Id.editText4);
            if (Preferences.ContainsKey("evening"))
            {
                eveningNotifyTime.SetText($"{Preferences.Get("evening", 18)}", TextView.BufferType.Normal);
            }
            var saveButton = (Button)FindViewById(Resource.Id.button1);
            saveButton.Click += new EventHandler(this.SaveSettings);
        }
        void SaveSettings(Object sender, EventArgs e)
        {
            double lon;
            double lat;
            int morningTime;
            int eveningTime;
            var correctLat = double.TryParse(latText.Text,out lat);
            if (correctLat)
            {
                if (-90 > lat || lat > 90)
                {
                    correctLat = false;
                }
            }
            var correctLon = double.TryParse(lonText.Text, out lon);
            if (correctLon)
            {
                if (-180 > lon || lon > 180)
                {
                    correctLat = false;
                }
            }
            var correctEveningTime = int.TryParse(eveningNotifyTime.Text, out eveningTime);
            if (correctEveningTime)
            {
                if (0 > eveningTime || 24 < eveningTime)
                {
                    correctEveningTime = false;
                }
            }
            var correctMorningTime = int.TryParse(morningNotifyTime.Text, out morningTime);
            if (correctMorningTime)
            {
                if (0 > morningTime || 24 < morningTime)
                {
                    correctMorningTime = false;
                }
            }
            if (correctLat && correctLon && correctMorningTime && correctEveningTime)
            {
                Preferences.Set("morning", morningTime);
                Preferences.Set("evening", eveningTime);
                Preferences.Set("lat", lat);
                Preferences.Set("lon", lon);
                Finish();
            }
            else
            {
                if (!correctLat)
                    Utilities.ShowMessage(sender, "Введите широту в указанном формате");
                if (!correctLon)
                    Utilities.ShowMessage(sender, "Введите долготу в указанном формате");
                if (!correctMorningTime)
                    Utilities.ShowMessage(sender, "Введите время утреннего уведомления в указанном формате");
                if (!correctEveningTime)
                    Utilities.ShowMessage(sender, "Введите время вечернего уведомления в указанном формате");
            }
        }
    }
}
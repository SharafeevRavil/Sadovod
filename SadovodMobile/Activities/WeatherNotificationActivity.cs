using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [Activity(Label ="Прогноз дождей", Theme ="@style/AppTheme.NoActionBar")]
    class WeatherNotificationActivity:AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string info;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                var response = client.GetAsync($"api/weather/getrain?lat={Preferences.Get("lat",0.0)}&lon={Preferences.Get("lon",0.0)}").Result;
                info = response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                info = "Требуется подключение к интернету";
            }
            SetContentView(Resource.Layout.WeatherNotification);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var textView = (TextView)FindViewById(Resource.Id.dynamicNotificationTextView);
            textView.SetText(info, TextView.BufferType.Normal);
            var button = (Button)FindViewById(Resource.Id.refreshButton);
            button.Click += new EventHandler(this.RefreshWeather);
            
        }
        private void RefreshWeather(Object sender,EventArgs e)
        {
                string info;
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                    var response = client.GetAsync($"api/weather/getrain?lat={Preferences.Get("lat", 0.0)}&lon={Preferences.Get("lon", 0.0)}").Result;
                    info = response.Content.ReadAsStringAsync().Result;
                }
                catch
                {
                    info = "Требуется подключение к интернету";
                }
                var textView = (TextView)FindViewById(Resource.Id.dynamicNotificationTextView);
                textView.SetText(info, TextView.BufferType.Normal);                        
        }
        public override void OnBackPressed()
        {
            var intent = new Intent();
            string token = Preferences.Get("token", null);
            try
            {
                if (token != null)
                {
                    //если есть токен, проверяю его валидность через getlogin
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    HttpResponseMessage response = client.GetAsync("/api/signup/getlogin").Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        intent = new Intent(this, typeof(SteadsActivity));
                    }

                    else
                    {
                        intent = new Intent(this, typeof(SignInActivity));
                    }
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    FinishAffinity();
                }
                else
                {
                    intent = new Intent(this, typeof(SignInActivity));
                }
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(intent);
                FinishAffinity();
            }
            catch
            {
                intent = new Intent(this, typeof(SignInActivity));
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(intent);
                FinishAffinity();
            }

        }
    }
}

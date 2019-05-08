using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace SadovodMobile.Activities
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            StartService(new Android.Content.Intent(this, typeof(MyService)));
            //await Task.Delay(2000); // Simulate a bit of startup work.
            RunOnUiThread(() =>
            {
                string token = Preferences.Get("token", null);
                if (token != null)
                {
                    //если есть токен, проверяю его валидность через getlogin
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    HttpResponseMessage response = client.GetAsync("/api/signup/getlogin").Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //если токен валиден
                        StartActivity(new Intent(Application.Context, typeof(SteadsActivity)));
                        UserSingleton.Instance.Token = token;
                    }
                    else
                    {
                        StartActivity(new Intent(Application.Context, typeof(SignInActivity)));
                    }
                }
                else
                {
                    StartActivity(new Intent(Application.Context, typeof(SignInActivity)));
                }
            });
        }
    }
}
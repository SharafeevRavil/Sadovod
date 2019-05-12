using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace SadovodMobile
{
    [Service/*(Name = "com.xamarin.sadovodmobile", Process = ":myprocess", Exported = true)*/]
    class MyService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        private Timer timerData;
        private void Debug()
        {
            timerData = new Timer((x) =>
            {
                Log.Debug("ZHOPA", "Debug service");
                if ((DateTime.Now.Hour == Preferences.Get("morning", 0) || DateTime.Now.Hour == Preferences.Get("evening", 0)) && (Preferences.Get("lastDayMorning", 0) < DateTime.Now.DayOfYear) || Preferences.Get("lastDayEvening", 0) < DateTime.Now.DayOfYear)
                {
                    if (DateTime.Now.Hour == Preferences.Get("morning", 0))
                        Preferences.Set("lastDayMorning", DateTime.Now.DayOfYear);
                    else
                        if (DateTime.Now.Hour == Preferences.Get("evening", 0))
                            Preferences.Set("lastDayEvening", DateTime.Now.DayOfYear);
                    SendPlanNotification();
                    SendWeatherNotification();
                }

            }, null, 0, 40000);
        }

        private void SendPlanNotification()
        {
            Intent resultIntent = new Intent(this, typeof(Activities.NotificationActivity));
            PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, resultIntent,
               PendingIntentFlags.UpdateCurrent);
            var info = Utilities.GetSteadsNotificationText();
            Notification.Builder notificationBuilder = new Notification.Builder(this)
            .SetSmallIcon(Resource.Drawable.Splash)
            .SetContentTitle("Задачи на сегодня")
            .SetStyle(new Notification.BigTextStyle()
                .BigText(info))
            .SetContentText(info)
            .SetContentIntent(resultPendingIntent);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(41442, notificationBuilder.Build());
        }
        private void SendWeatherNotification()
        {
            try
            {
                Intent weatherIntent = new Intent(this, typeof(Activities.WeatherNotificationActivity));
                PendingIntent weatherPendingIntent = PendingIntent.GetActivity(this, 0, weatherIntent,
                               PendingIntentFlags.UpdateCurrent);
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
                var response = client.GetAsync($"api/weather/getrain?lat={Preferences.Get("lat", 0.0)}&lon={Preferences.Get("lon", 0.0)}").Result;
                var info = response.Content.ReadAsStringAsync().Result;
                Notification.Builder notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.Splash)
                .SetContentTitle("Прогноз дождей")
                .SetStyle(new Notification.BigTextStyle()
                    .BigText(info))
                .SetContentText(info)
                .SetContentIntent(weatherPendingIntent);
                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.Notify(41441, notificationBuilder.Build());
            }
            catch
            {
                Intent weatherIntent = new Intent(this, typeof(Activities.WeatherNotificationActivity));
                PendingIntent weatherPendingIntent = PendingIntent.GetActivity(this, 0, weatherIntent,
                               PendingIntentFlags.UpdateCurrent);
                Notification.Builder notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.Splash)
                .SetContentTitle("Прогноз дождей")
                .SetStyle(new Notification.BigTextStyle()
                    .BigText("Требуется подключение к интернету"))
                .SetContentText("Требуется подключение к интернету")
                .SetContentIntent(weatherPendingIntent);
            }

        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug("ZHOPA", "Successfully started");
            Debug();
            return StartCommandResult.Sticky;
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }
    }
}
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
                
                SendNotification();
            }, null, 0, 10000);
        }
        
        private void SendNotification()
        {
            Intent resultIntent = new Intent(this, typeof(Activities.NotificationActivity));
PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, resultIntent,
               PendingIntentFlags.UpdateCurrent);
        var info = Utilities.GetSteadsNotificationText();
            Notification.Builder notificationBuilder = new Notification.Builder(this)
            .SetSmallIcon(Resource.Drawable.Splash)
            .SetContentTitle("Задачи на сегодня:")
            .SetStyle(new Notification.BigTextStyle()
                .BigText(info))
            .SetContentText(info)
            .SetContentIntent(resultPendingIntent);

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(41441, notificationBuilder.Build());
        }

        public static void Parse()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMjA4OTQ0ODciLCJleHAiOjE1NTc0MzEwNjd9.DAn-3drgDnUumHotJVP-gPkgAsFKl-ufiLiyxyy3bVA");

            string json1 = $"'{JsonConvert.SerializeObject($"{DateTime.Now}")}'";
            var content2 = new StringContent(json1, Encoding.UTF8, "application/json");
            int id = 52;
            HttpResponseMessage response = client.PutAsync($"/api/database/DatabaseUpdateStead?id={id}", content2).Result;

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
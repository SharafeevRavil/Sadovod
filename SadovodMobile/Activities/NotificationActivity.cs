﻿using System;
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
    [Activity(Label ="Задачи на сегодня", Theme = "@style/AppTheme.NoActionBar")]
    class NotificationActivity: AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            var info = Utilities.GetSteadsNotificationText();
            SetContentView(Resource.Layout.MyNotificationLayout);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
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
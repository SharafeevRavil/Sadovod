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
    [Activity(Label = "Войдите в аккаунт", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.SignIn);

            // Create your application here
            Button SignInButton = FindViewById<Button>(Resource.Id.button1);
            SignInButton.Click += SignInOnClick;
        }

        private void SignInOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "ПИДОР, ты нажал кнопку ВОЙТИ", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
    }
}
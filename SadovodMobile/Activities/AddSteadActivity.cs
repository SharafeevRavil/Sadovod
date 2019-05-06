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
using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Добавить участок", Theme = "@style/AppTheme.NoActionBar")]
    public class AddSteadActivity : AppCompatActivity
    {
        private EditText text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddStead);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Привязка кнопки добавления участка
            FindViewById<Button>(Resource.Id.button1).Click += AddSteadAction;
            //Привязка текста с именем участка
            text = FindViewById<EditText>(Resource.Id.editText1);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.logOut)
            {
                UserSingleton.Instance.LogOut();
                Intent intent = new Intent(this, typeof(SignInActivity));
                FinishAffinity();
                StartActivity(intent);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        public void AddSteadAction(object sender, EventArgs eventArgs)
        {
            string name = text.Text;
            if(name == null || name.Length == 0 /*|| !Utilities.IsFromLatinOrNums(name)*/)
            {
                //если название говно
                Utilities.ShowMessage(sender, "Название не должно быть пустым");
            }
            else
            {
                //если все ок, то добавляем участок
                UserSingleton.Instance.AddStead(new Stead(name));
                Finish();
            }
        }
    }
}
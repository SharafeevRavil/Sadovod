using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Войдите в аккаунт", Theme = "@style/AppTheme.NoActionBar")]
    public class SignInActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Привязка кнопки входа
            FindViewById<Button>(Resource.Id.button1).Click += SignInOnClickAsync;
            //Привязка кнопки регистрации
            FindViewById<Button>(Resource.Id.button2).Click += SignUpOnClick;
        }
        public class UserDto
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class SignedIn
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Token { get; set; }
        }
        //нажатие на кнопку входа
        private async void SignInOnClickAsync(object sender, EventArgs eventArgs)
        {
            string username = FindViewById<EditText>(Resource.Id.editText1).Text;
            string password = FindViewById<EditText>(Resource.Id.editText2).Text;

            //FIXME::
            //ТУТ Я АВТОРИЗУЮСЬ

            //Если был успешно авторизован, то переключаю на экран грядок
            //Иначе снизу всплывает сообщение о неправильных данных

            //Делаю запрос https://sadovodhelperexample.azurewebsites.net/api/signup/Authenticate
            //С телом вида {"Username":"penis1","Password":"password"}
            /*HttpClient client = new HttpClient();
            UserDto dto = new UserDto() { Username = username, Password = password };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(dto));
            //HttpContent content = new StringContent($"{{\"Username\":\"{username}\",\"Password\":\"{password}\"}}");
            HttpResponseMessage response = await client.PostAsync(
                "https://sadovodhelperexample.azurewebsites.net/api/signup/Authenticate", content);
            //request.Headers.Add("Accept", "application/json");
            */

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            UserDto dto = new UserDto() { Username = username, Password = password };
            string json = $"'{JsonConvert.SerializeObject(dto)}'";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("/api/signup/Authenticate", content).Result;
            //HttpResponseMessage response1 = await client.GetAsync("/api/database/DatabaseGetByGardenerID?id=5");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = response.Content.ReadAsStringAsync().Result;
                UserSingleton.Instance.Token = token;
                
                //Переключаю на экран участков
                Intent intent = new Intent(this, typeof(SteadsActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Utilities.ShowMessage(sender,"Неправильные данные входа");
            }
        }
        //нажатие на кнопку регистрации
        private void SignUpOnClick(object sender, EventArgs eventArgs)
        {
            //Переключаю на экран регистрации
            Intent intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
        }
    }
}
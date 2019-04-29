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
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Войдите в аккаунт", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);

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

            //string jsonData = @"{""username"" : ""myusername"", ""password"" : ""mypassword""}"

            UserDto dto = new UserDto() { Username = username, Password = password };
            string json = $"'{JsonConvert.SerializeObject(dto)}'";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/signup/Authenticate", content);
            HttpResponseMessage response1 = await client.GetAsync("/api/database/DatabaseGetByGardenerID?id=5");
            var res1 = response1.Content;

            var token = await response.Content.ReadAsStringAsync();
            //var signed = JsonConvert.DeserializeObject<SignedIn>(resp);

            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            var result = response.Content;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                //var json = await responseContent.ReadAsStringAsync();
                var ans = responseContent.ToString();

                //Переключаю на экран участков
                Intent intent = new Intent(this, typeof(SteadsActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                View view = (View)sender;
                Snackbar
                    .Make(view, "Неправильные данные входа", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
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
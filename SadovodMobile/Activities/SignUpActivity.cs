using System;
using System.Collections.Generic;
using System.Linq;
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
using static SadovodMobile.Activities.SignInActivity;

namespace SadovodMobile.Activities
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);

            //Привязка кнопки регистрации
            FindViewById<Button>(Resource.Id.button1).Click += SignUpOnClickAsync;
        }

        private bool CheckSignUpData(object sender, string login, string email, string pass1, string pass2)
        {
            if (login == null || login.Length == 0)
            {
                Utilities.ShowMessage(sender, "Не указано имя пользователя");
                return false;
            }
            if (email == null || email.Length == 0)
            {
                Utilities.ShowMessage(sender, "Не указан адрес электронной почты");
                return false;
            }
            if (pass1 == null || pass1.Length == 0)
            {
                Utilities.ShowMessage(sender, "Не указан пароль");
                return false;
            }
            if (pass2 == null || pass2.Length == 0)
            {
                Utilities.ShowMessage(sender, "Не указан повтор пароля");
                return false;
            }


            if (!Utilities.IsFromLatinOrNums(login))
            {
                Utilities.ShowMessage(sender, "Имя пользователя должно состоять из латинских букв или цифр");
                return false;
            }
            if (!Utilities.IsFromLatinOrNums(pass1))
            {
                Utilities.ShowMessage(sender, "Пароль должен состоять из латинских букв или цифр");
                return false;
            }

            if (pass1 != pass2)
            {
                Utilities.ShowMessage(sender, "Пароли не совпадают");
                return false;
            }
            return true;
        }
        //нажатие на кнопку регистрации
        private async void SignUpOnClickAsync(object sender, EventArgs eventArgs)
        {
            string login = FindViewById<EditText>(Resource.Id.editText1).Text;
            string email = FindViewById<EditText>(Resource.Id.editText2).Text;
            string pass1 = FindViewById<EditText>(Resource.Id.editText3).Text;
            string pass2 = FindViewById<EditText>(Resource.Id.editText4).Text;
            
            if (!CheckSignUpData(sender, login, email, pass1, pass2))
            {
                return;
            }

            //FIXME::
            //ТУТ Я ПЫТАЮСЬ ЗАРЕГИСТРИРОВАТЬСЯ
            //Если был успешно авторизован, то авторизуюсь(если нужно будет) и переключаю на экран участков
            //Иначе снизу всплывает сообщение о неудачной регистрации

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");


            UserDto dto1 = new UserDto() { Username = login, Password = pass1 };
            string json1 = $"'{JsonConvert.SerializeObject(dto1)}'";
            var content2 = new StringContent(json1, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await client.PostAsync("/api/signup/Register", content2);

            if(response1.StatusCode == System.Net.HttpStatusCode.OK){
                UserDto dto = new UserDto() { Username = login, Password = pass1 };
                string json = $"'{JsonConvert.SerializeObject(dto)}'";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/api/signup/Authenticate", content);
                var token = await response.Content.ReadAsStringAsync();
                UserSingleton.Instance.Token = token;
                //Переключаю на экран участков
                Intent intent = new Intent(this, typeof(SteadsActivity));
                FinishAffinity();
                StartActivity(intent);
            }
            else
            {
                Utilities.ShowMessage(sender, "Регистрация не удалась, попробуйте позже");
            }
        }
    }
}
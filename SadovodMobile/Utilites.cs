using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace SadovodMobile
{
    static class Utilites
    {
        public static T GetDeserializeJson<T>(string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://sadovodhelper.azurewebsites.net/");
            var response = client.GetAsync(path).Result;
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SadovodClasses;

namespace SadovodMobile
{
    public class UserSingleton
    {
        private static UserSingleton instance;
        public static UserSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    Instance = new UserSingleton();
                }
                return instance;
            }
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }

        private string token;
        public string Token {
            get
            {
                return token;
            }
            set
            {
                token = value;
                penisAsync();
            }
        }

        public async void penisAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = await client.GetAsync("/api/database/DatabaseGetByGardenerID");
            string stead = await response.Content.ReadAsStringAsync();
        }

        public UserSingleton()
        {
            //FIXME:: делать загрузку информации о юзере с бека
            steads = new List<Stead>();
        }

        private List<Stead> steads;
        public ReadOnlyCollection<Stead> Steads
        {
            get => new ReadOnlyCollection<Stead>(steads);
        }

        public void AddStead(Stead stead)
        {
            steads.Add(stead);

            postStead(stead);

            SteadsChanged.Invoke(this, new EventArgs());
        }

        public async void postStead(Stead stead)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

            string json1 = $"'{JsonConvert.SerializeObject(stead)}'";
            var content2 = new StringContent(json1, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await client.PostAsync("/api/database/DatabasePostStead", content2);
        }

        public event EventHandler SteadsChanged;

        private Stead currentStead;
        public Stead CurrentStead
        {
            get
            {
                return currentStead;
            }
            set
            {
                if (steads.Contains(value))
                {
                    currentStead = value;
                }
            }
        }

        private GardenBed currentBed;
        public GardenBed CurrentBed
        {
            get
            {
                return currentBed;
            }
            set
            {
                if (CurrentStead.GardenBeds.Contains(value))
                {
                    currentBed = value;
                }
            }
        }
    }
}
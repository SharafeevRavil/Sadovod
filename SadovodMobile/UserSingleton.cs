using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
using Xamarin.Essentials;

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
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                Preferences.Set("token", token);
                GetAllSteads();
            }
        }

        public async void GetAllSteads()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = client.GetAsync("/api/database/DatabaseGetByGardenerID").Result;
            string mySteads = response.Content.ReadAsStringAsync().Result;
            var a = JsonConvert.DeserializeObject<List<DatabaseStead>>(mySteads);

            databaseSteads = new List<DatabaseStead>();
            steads = new List<Stead>();
            currentBed = null;
            currentStead = null;

            foreach (var b in a)
            {
                databaseSteads.Add(b);
                steads.Add(b.Stead);
                b.Stead.BedsChanged += UpdateSteadAsync;
            }
            SteadsChanged?.Invoke(this, new EventArgs());
        }

        public UserSingleton()
        {
            //FIXME:: делать загрузку информации о юзере с бека
            steads = new List<Stead>();
            databaseSteads = new List<DatabaseStead>();
        }

        public async void UpdateSteadAsync(object sender, EventArgs args)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            string json1 = $"'{JsonConvert.SerializeObject(CurrentStead)}'";
            var content2 = new StringContent(json1, Encoding.UTF8, "application/json");
            int id = databaseSteads.Where(x => x.Stead == currentStead).First().Id;
            HttpResponseMessage response = await client.PutAsync($"/api/database/DatabaseUpdateStead?id={id}", content2);
        }

        private List<Stead> steads;
        public ReadOnlyCollection<Stead> Steads
        {
            get => new ReadOnlyCollection<Stead>(steads);
        }

        private List<DatabaseStead> databaseSteads;

        public void AddStead(Stead stead)
        {
            steads.Add(stead);
            stead.BedsChanged += UpdateSteadAsync;

            postStead(stead);

            SteadsChanged.Invoke(this, new EventArgs());
        }

        public async void RemoveSteadAtAsync(int position)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = client.DeleteAsync($"/api/database/DatabaseDeleteStead?id={databaseSteads[position].Id}").Result;
            databaseSteads.RemoveAt(position);
            steads.RemoveAt(position);
            SteadsChanged.Invoke(this, new EventArgs());
        }

        public async void postStead(Stead stead)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://sadovodhelperexample.azurewebsites.net");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

            string json1 = $"'{JsonConvert.SerializeObject(stead)}'";
            var content2 = new StringContent(json1, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/database/DatabasePostStead", content2);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                int globalId = int.Parse(await response.Content.ReadAsStringAsync());
                databaseSteads.Add(new DatabaseStead() { Id = globalId, Stead = stead });
            }
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
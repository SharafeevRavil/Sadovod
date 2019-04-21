using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
                if(instance == null)
                {
                    instance = value;
                }
            }
        }

        public UserSingleton()
        {
            //FIXME:: делать загрузку информации о юзере с бека
            steads = new List<Stead>();
        }

        private static List<Stead> steads;
        public static ReadOnlyCollection<Stead> Steads
        {
            get => new ReadOnlyCollection<Stead>(steads);
        }

        private static Stead currentStead;
        public static Stead CurrentStead
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

        private static GardenBed currentBed;
        public static GardenBed CurrentBed
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
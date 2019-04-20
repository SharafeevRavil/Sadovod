using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "GardenBedsActivity")]
    public class BedsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //FIXME:: Нужно указать лаяут грядок
            SetContentView(Resource.Layout.GardenBeds);

            //Привязка кнопки добавления грядки
            FindViewById<Button>(Resource.Id.button1).Click += AddBedAction;
            //Инициализация всех грядок пользователя
            InitializeBeds();
        }
        //Метод действия добавления грядки
        public void AddBedAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления грядки
            //FIXME:: Нужно написать и указать активити добавления грядки
            Intent intent = new Intent(this, typeof(AddBedActivity));
            StartActivity(intent);
        }

        //Метод инициализации грядок
        private void InitializeBeds()
        {
            //FIXME:: Нужно откуда-то получать все грядки пользователя
            /*
            foreach (GardenBed bed in USER.BEDS)
            {
                AddBed(bed);
            }*/
        }
        //Метод добавления грядок
        private void AddBed(GardenBed bed)
        {
            //FIXME:: Нужно добавлять элементы грядок в layout

            //FIXME:: Нужно добавить действие при нажатии на кнопку добавленной грядки

            //FIXME:: Нужно добавить действие при нажатии на кнопки действий в грядке
        }
    }
}
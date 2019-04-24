﻿using System;
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
            Intent intent = new Intent(this, typeof(AddBedActivity));
            StartActivity(intent);
        }

        //Метод инициализации грядок
        private void InitializeBeds()
        {
            foreach (GardenBed bed in UserSingleton.Instance.CurrentStead.GardenBeds)
            {
                AddBed(bed);
            }
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
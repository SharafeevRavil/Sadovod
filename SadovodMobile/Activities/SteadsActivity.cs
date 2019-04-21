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
    [Activity(Label = "SteadsActivity")]
    public class SteadsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Steads);

            //Привязка кнопки добавления участка
            FindViewById<Button>(Resource.Id.button1).Click += AddSteadAction;
            //Инициализация всех участков пользователя
            InitializeSteads();
        }

        //Метод действия добавления участка
        public void AddSteadAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления участка
            Intent intent = new Intent(this, typeof(AddSteadActivity));
            StartActivity(intent);
        }

        //Метод инициализации участков
        private void InitializeSteads()
        {
            foreach(Stead stead in UserSingleton.Instance.Steads)
            {
                AddStead(stead);
            }
        }
        //Метод добавления грядок
        private void AddStead(Stead stead)
        {
            //FIXME:: Нужно добавлять элементы участков в layout
            
            //FIXME:: Нужно добавить действие при нажатии на кнопку добавленного участка
        }
    }
}
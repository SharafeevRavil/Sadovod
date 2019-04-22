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
    [Activity(Label = "BedActivity")]
    public class AddBedActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddGardenBed);

            //Привязка кнопки добавления записки
            FindViewById<Button>(Resource.Id.button1).Click += AddNoteAction;
            //Привязка кнопки добавления грядки
            FindViewById<Button>(Resource.Id.button2).Click += AddBedAction;

            //Инициализация всех полей
            Initialize();
        }
        //Инициализация всех полей
        private void Initialize()
        {
            //FIXME:: Получать данные из грядки и отрисовывать их(изменить начальный текст)
            //FIXME:: Отрисовать все записки
        }

        //Действие добавления записки
        public void AddNoteAction(object sender, EventArgs eventArgs)
        {
            //FIXME:: Добавлять записку
        }
        //Действие добавления грядки
        public void AddBedAction(object sender, EventArgs eventArgs)
        {
            //Поле вида растения
            EditText typeName = FindViewById<EditText>(Resource.Id.editText1);
            //Поле сорта растения
            EditText sortName = FindViewById<EditText>(Resource.Id.editText2);
            //Поле даты полива
            EditText waterDate = FindViewById<EditText>(Resource.Id.editText3);
            //Поле периодичности полива
            EditText waterPeriod = FindViewById<EditText>(Resource.Id.editText4);
            //Поле даты прополки
            EditText weedDate = FindViewById<EditText>(Resource.Id.editText5);
            //Поле периодичности прополки
            EditText weedPeriod = FindViewById<EditText>(Resource.Id.editText6);
            //Поле даты окучивания
            EditText pillUpDate = FindViewById<EditText>(Resource.Id.editText7);
            //Поле периодичности окучивания
            EditText pillUpPeriod = FindViewById<EditText>(Resource.Id.editText8);
            //Поле даты удобрения
            EditText fertilizeDate = FindViewById<EditText>(Resource.Id.editText9);
            //Поле периодичности удобрения
            EditText fertilizePeriod = FindViewById<EditText>(Resource.Id.editText10);

            //FIXME:: Беру все эти поля нахуй и еще записки в придачу и хуярю все в грядку

            UserSingleton.Instance.CurrentStead.AddBed(new GardenBed(new PlantType(typeName.Text, sortName.Text)));
            Finish();
        }

    }
}
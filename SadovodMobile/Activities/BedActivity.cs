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

namespace SadovodMobile.Activities
{
    [Activity(Label = "BedActivity")]
    public class BedActivity : Activity
    {
        private EditText typeName;
        private EditText sortName;
        private EditText waterDate;
        private EditText waterPeriod;
        private EditText weedDate;
        private EditText weedPeriod;
        private EditText pillUpDate;
        private EditText pillUpPeriod;
        private EditText fertilizeDate;
        private EditText fertilizePeriod;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GardenBed);

            //Привязка поля вида растения
            typeName = FindViewById<EditText>(Resource.Id.editText1);
            typeName.KeyPress += TypeNameChanged;
            //Привязка поля сорта растения
            sortName = FindViewById<EditText>(Resource.Id.editText2);
            sortName.KeyPress += SortNameChanged;

            //Привязка поля даты полива
            waterDate = FindViewById<EditText>(Resource.Id.editText3);
            waterDate.KeyPress += TypeNameChanged;
            //Привязка поля периодичности полива
            waterPeriod = FindViewById<EditText>(Resource.Id.editText4);
            waterPeriod.KeyPress += SortNameChanged;

            //Привязка поля даты прополки
            weedDate = FindViewById<EditText>(Resource.Id.editText5);
            weedDate.KeyPress += TypeNameChanged;
            //Привязка поля периодичности прополки
            weedPeriod = FindViewById<EditText>(Resource.Id.editText6);
            weedPeriod.KeyPress += SortNameChanged;

            //Привязка поля даты окучивания
            pillUpDate = FindViewById<EditText>(Resource.Id.editText7);
            pillUpDate.KeyPress += TypeNameChanged;
            //Привязка поля периодичности окучивания
            pillUpPeriod = FindViewById<EditText>(Resource.Id.editText8);
            pillUpPeriod.KeyPress += SortNameChanged;

            //Привязка поля даты удобрения
            fertilizeDate = FindViewById<EditText>(Resource.Id.editText9);
            fertilizeDate.KeyPress += TypeNameChanged;
            //Привязка поля периодичности удобрения
            fertilizePeriod = FindViewById<EditText>(Resource.Id.editText10);
            fertilizePeriod.KeyPress += SortNameChanged;

            //Инициализация всех полей
            Initialize();
        }
        //Инициализация всех полей
        private void Initialize()
        {
            //FIXME:: Получать данные из грядки и отрисовывать их(изменить начальный текст)
            //FIXME:: Отрисовать все записки
        }

        //Действие при изменении вида растения
        public void TypeNameChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
        //Действие при изменении сорта растения
        public void SortNameChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }

        //Действие при изменении даты полива
        public void WaterDateChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
        //Действие при изменении периодичности полива
        public void WaterPeriodChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }

        //Действие при изменении даты прополки
        public void WeedDateChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
        //Действие при изменении периодичности прополки
        public void WeedPeriodChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }

        //Действие при изменении даты окучивания
        public void PileUpDateChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
        //Действие при изменении периодичности окучивания
        public void PileUpPeriodChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }

        //Действие при изменении даты удобрения
        public void FertilizeDateChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
        //Действие при изменении периодичности удобрения
        public void FertilizePeriodChanged(object sender, EventArgs eventArgs)
        {
            //FIXME:: получать данные из поля и изменять их у грядки
        }
    }
}
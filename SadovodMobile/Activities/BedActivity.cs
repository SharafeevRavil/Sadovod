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
    [Activity(Label = "BedActivity")]
    public class BedActivity : Activity
    {
        private EditText typeName;
        private EditText sortName;
        private EditText waterDate;
        private EditText waterPeriod;
        private EditText weedDate;
        private EditText weedPeriod;
        private EditText pileUpDate;
        private EditText pileUpPeriod;
        private EditText fertilizeDate;
        private EditText fertilizePeriod;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GardenBed);

            //Привязка поля вида растения
            typeName = FindViewById<EditText>(Resource.Id.editText1);
            //Привязка поля сорта растения
            sortName = FindViewById<EditText>(Resource.Id.editText2);
            //Привязка поля даты полива
            waterDate = FindViewById<EditText>(Resource.Id.editText3);
            //Привязка поля периодичности полива
            waterPeriod = FindViewById<EditText>(Resource.Id.editText4);
            //Привязка поля даты прополки
            weedDate = FindViewById<EditText>(Resource.Id.editText5);
            //Привязка поля периодичности прополки
            weedPeriod = FindViewById<EditText>(Resource.Id.editText6);
            //Привязка поля даты окучивания
            pileUpDate = FindViewById<EditText>(Resource.Id.editText7);
            //Привязка поля периодичности окучивания
            pileUpPeriod = FindViewById<EditText>(Resource.Id.editText8);
            //Привязка поля даты удобрения
            fertilizeDate = FindViewById<EditText>(Resource.Id.editText9);
            //Привязка поля периодичности удобрения
            fertilizePeriod = FindViewById<EditText>(Resource.Id.editText10);

            //Инициализация всех полей
            Initialize();

            typeName.TextChanged += TypeNameChanged;
            sortName.TextChanged += SortNameChanged;
            waterDate.TextChanged += WaterDateChanged;
            waterPeriod.TextChanged += WaterPeriodChanged;
            weedDate.TextChanged += WeedDateChanged;
            weedPeriod.TextChanged += WeedPeriodChanged;
            pileUpDate.TextChanged += PileUpDateChanged;
            pileUpPeriod.TextChanged += PileUpPeriodChanged;
            fertilizeDate.TextChanged += FertilizeDateChanged;
            fertilizePeriod.TextChanged += FertilizePeriodChanged;
        }
        //Инициализация всех полей
        private void Initialize()
        {
            GardenBed bed = UserSingleton.Instance.CurrentBed;
            typeName.Text = bed.Plant.TypeName;
            sortName.Text = bed.Plant.SortName;
            waterDate.Text = bed.WaterDate.ToString("dd/MM/yyyy hh:mm");
            waterPeriod.Text = bed.WaterPeriod.ToString();
            weedDate.Text = bed.WeedDate.ToString("dd/MM/yyyy hh:mm");
            weedPeriod.Text = bed.WeedPeriod.ToString();
            pileUpDate.Text = bed.PileUpDate.ToString("dd/MM/yyyy hh:mm");
            pileUpPeriod.Text = bed.PileUpPeriod.ToString();
            fertilizeDate.Text = bed.FertilizeDate.ToString("dd/MM/yyyy hh:mm");
            fertilizePeriod.Text = bed.FertilizePeriod.ToString();
            //FIXME:: Отрисовать все записки
        }

        //Действие при изменении вида растения
        public void TypeNameChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.Plant.TypeName = typeName.Text;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Действие при изменении сорта растения
        public void SortNameChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.Plant.SortName = sortName.Text;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }

        //Действие при изменении даты полива
        public void WaterDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.WaterDate = Utilities.DateTimeFormat(waterDate.Text);
        }
        //Действие при изменении периодичности полива
        public void WaterPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(waterPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.WaterPeriod = result;
            }
        }

        //Действие при изменении даты прополки
        public void WeedDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.WeedDate = Utilities.DateTimeFormat(weedDate.Text);
        }
        //Действие при изменении периодичности прополки
        public void WeedPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(weedPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.WeedPeriod = result;
            }
        }

        //Действие при изменении даты окучивания
        public void PileUpDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.PileUpDate = Utilities.DateTimeFormat(pileUpDate.Text);
        }
        //Действие при изменении периодичности окучивания
        public void PileUpPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(pileUpPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.PileUpPeriod = result;
            }
        }

        //Действие при изменении даты удобрения
        public void FertilizeDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.FertilizeDate = Utilities.DateTimeFormat(fertilizeDate.Text);
        }
        //Действие при изменении периодичности удобрения
        public void FertilizePeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(fertilizePeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.FertilizePeriod = result;
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Мои грядки", Theme = "@style/AppTheme.NoActionBar")]
    public class BedsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GardenBeds);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Привязка кнопки добавления грядки
            FindViewById<Button>(Resource.Id.button1).Click += AddBedAction;
            //Инициализация всех грядок пользователя
            InitializeBeds();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.logOut)
            {
                UserSingleton.Instance.LogOut();
                Intent intent = new Intent(this, typeof(SignInActivity));
                FinishAffinity();
                StartActivity(intent);
                return true;
            }
            if (id == Resource.Id.settings)
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
                return true;
            }
            if (id == Resource.Id.rain)
            {
                Intent intenet = new Intent(this, typeof(WeatherNotificationActivity));
                StartActivity(intenet);
                return true;
            }
            if (id == Resource.Id.tasks)
            {
                Intent intenet = new Intent(this, typeof(NotificationActivity));
                StartActivity(intenet);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        //Метод действия добавления грядки
        public void AddBedAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления грядки
            Intent intent = new Intent(this, typeof(AddBedActivity));
            StartActivity(intent);
        }

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        BedsAdapter mAdapter;
        //Метод инициализации грядок
        private void InitializeBeds()
        {
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new BedsAdapter(UserSingleton.Instance.CurrentStead.GardenBeds, this);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.ItemClick += OnBedClick;
            mAdapter.WaterClick += OnWaterClick;
            mAdapter.WeedClick += OnWeedClick;
            mAdapter.PileUpClick += OnPileUpClick;
            mAdapter.FertilizeClick += OnFertilizeClick;

            UserSingleton.Instance.CurrentStead.BedsChanged += OnCollectionChanged;
            RegisterForContextMenu(mRecyclerView);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            IMenuItem delete = menu.Add(Menu.None, 0, 0, "Удалить грядку");
            delete.SetOnMenuItemClickListener(new MenuClick(this));
        }

        public int MenuPosition { get; set; }
        public class MenuClick : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            private BedsActivity bedsActivity;

            public MenuClick(BedsActivity bedsActivity)
            {
                this.bedsActivity = bedsActivity;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                switch (item.ItemId)
                {
                    case 0:
                        UserSingleton.Instance.CurrentStead.RemoveBed(bedsActivity.MenuPosition);
                        return true;
                    default:
                        return false;
                }
            }
        }

        //Событие изменения коллекции грядок
        public void OnCollectionChanged(object sender, EventArgs eventArgs)
        {
            mAdapter.NotifyDataSetChanged();
        }
        //Событие нажатия на грядку
        private void OnBedClick(object sender, int position)
        {
            //Toast.MakeText(this, $"This is bed {position + 1}", ToastLength.Short).Show();
            UserSingleton.Instance.CurrentBed = UserSingleton.Instance.CurrentStead.GardenBeds[position];
            //Переключаюсь на окно редактирования грядки
            Intent intent = new Intent(this, typeof(BedActivity));
            StartActivity(intent);
        }
        //Событие нажатия на полив
        private void OnWaterClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].WaterDate = DateTime.Now;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Событие нажатия на прополку
        private void OnWeedClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].WeedDate = DateTime.Now;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Событие нажатия на окучивание
        private void OnPileUpClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].PileUpDate = DateTime.Now;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Событие нажатия на удобрение
        private void OnFertilizeClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].FertilizeDate = DateTime.Now;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }

        public class BedsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<int> ItemClick;
            public event EventHandler<int> WaterClick;
            public event EventHandler<int> WeedClick;
            public event EventHandler<int> PileUpClick;
            public event EventHandler<int> FertilizeClick;
            public ReadOnlyCollection<GardenBed> Beds;

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.BedView, parent, false);
                BedViewHolder vh = new BedViewHolder(itemView, OnClick, OnWaterClick, OnWeedClick, OnPileUpClick, OnFertilizeClick, OnLongClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                BedViewHolder vh = holder as BedViewHolder;
                vh.TypeName.Text = Beds[position].Plant.TypeName;
                vh.SortName.Text = Beds[position].Plant.SortName;

                Color falseColor = Color.OrangeRed;
                Color trueColor = new Color(0x8B, 0xC3, 0x4A);
                vh.Button1.SetColorFilter(new PorterDuffColorFilter(
                    Utilities.CheckActionNeed(Beds[position].WaterDate, DateTime.Now, Beds[position].WaterPeriod) ? falseColor : trueColor,
                    PorterDuff.Mode.DstAtop));
                vh.Button2.SetColorFilter(new PorterDuffColorFilter(
                    Utilities.CheckActionNeed(Beds[position].WeedDate, DateTime.Now, Beds[position].WeedPeriod) ? falseColor : trueColor,
                    PorterDuff.Mode.DstAtop));
                vh.Button3.SetColorFilter(new PorterDuffColorFilter(
                    Utilities.CheckActionNeed(Beds[position].PileUpDate, DateTime.Now, Beds[position].PileUpPeriod) ? falseColor : trueColor,
                    PorterDuff.Mode.DstAtop));
                vh.Button4.SetColorFilter(new PorterDuffColorFilter(
                    Utilities.CheckActionNeed(Beds[position].FertilizeDate, DateTime.Now, Beds[position].FertilizePeriod) ? falseColor : trueColor,
                    PorterDuff.Mode.DstAtop));
            }

            public override int ItemCount
            {
                get { return Beds.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            void OnWaterClick(int position)
            {
                WaterClick?.Invoke(this, position);
            }
            void OnWeedClick(int position)
            {
                WeedClick?.Invoke(this, position);
            }
            void OnPileUpClick(int position)
            {
                PileUpClick?.Invoke(this, position);
            }
            void OnFertilizeClick(int position)
            {
                FertilizeClick?.Invoke(this, position);
            }

            private BedsActivity mActivity;
            public BedsAdapter(ReadOnlyCollection<GardenBed> beds, BedsActivity activity)
            {
                Beds = beds;
                mActivity = activity;
            }
            void OnLongClick(object sender, int position)
            {
                mActivity.MenuPosition = position;
                mActivity.mRecyclerView.ShowContextMenu();
            }
        }

        public class BedViewHolder : RecyclerView.ViewHolder
        {
            public LinearLayout MainBedLayout { get; private set; }
            public TextView TypeName { get; private set; }
            public TextView SortName { get; private set; }

            public ImageButton Button1 { get; private set; }
            public ImageButton Button2 { get; private set; }
            public ImageButton Button3 { get; private set; }
            public ImageButton Button4 { get; private set; }

            public BedViewHolder(View itemView, Action<int> listener, Action<int> waterListener, Action<int> weedListener,
                Action<int> pileUpListener, Action<int> fertilizeListener, Action<object, int> longListener) : base(itemView)
            {
                // Locate and cache view references:
                MainBedLayout = itemView.FindViewById<LinearLayout>(Resource.Id.bedLinLayout);
                MainBedLayout.Click += (sender, e) => listener(LayoutPosition);
                MainBedLayout.LongClick += (sender, e) => longListener(sender, LayoutPosition);

                TypeName = itemView.FindViewById<TextView>(Resource.Id.textView1);
                SortName = itemView.FindViewById<TextView>(Resource.Id.textView2);

                Button1 = itemView.FindViewById<ImageButton>(Resource.Id.imageButton1);
                Button2 = itemView.FindViewById<ImageButton>(Resource.Id.imageButton2);
                Button3 = itemView.FindViewById<ImageButton>(Resource.Id.imageButton3);
                Button4 = itemView.FindViewById<ImageButton>(Resource.Id.imageButton4);

                Button1.Click += (sender, r) => waterListener(LayoutPosition);
                Button2.Click += (sender, r) => weedListener(LayoutPosition);
                Button3.Click += (sender, r) => pileUpListener(LayoutPosition);
                Button4.Click += (sender, r) => fertilizeListener(LayoutPosition);
            }
        }
    }
}
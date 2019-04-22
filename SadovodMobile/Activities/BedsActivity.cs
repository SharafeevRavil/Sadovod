using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
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
            mAdapter = new BedsAdapter(UserSingleton.Instance.CurrentStead.GardenBeds);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.ItemClick += OnBedClick;
            UserSingleton.Instance.CurrentStead.BedsChanged += OnCollectionChanged;
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

        public class BedsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<int> ItemClick;
            public ReadOnlyCollection<GardenBed> Beds;
            public BedsAdapter(ReadOnlyCollection<GardenBed> steads)
            {
                Beds = steads;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.SteadView, parent, false);
                SteadViewHolder vh = new SteadViewHolder(itemView, OnClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                SteadViewHolder vh = holder as SteadViewHolder;
                vh.Button.Text = Beds[position].Plant.TypeName;
            }

            public override int ItemCount
            {
                get { return Beds.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }
        }
        public class SteadViewHolder : RecyclerView.ViewHolder
        {
            public Button Button { get; private set; }

            public SteadViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                // Locate and cache view references:
                Button = itemView.FindViewById<Button>(Resource.Id.button);
                Button.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }
    }
}
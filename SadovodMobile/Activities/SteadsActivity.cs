using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "Ваши участки", Theme = "@style/AppTheme.NoActionBar")]
    public class SteadsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Steads);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Привязка кнопки добавления участка
            FindViewById<Button>(Resource.Id.button1).Click += AddSteadAction;
            //Инициализация всех участков пользователя
            InitializeSteads();
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

        //Метод действия добавления участка
        public void AddSteadAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления участка
            Intent intent = new Intent(this, typeof(AddSteadActivity));
            StartActivity(intent);
        }

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        SteadsAdapter mAdapter;
        //Метод инициализации участков
        private void InitializeSteads()
        {
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new SteadsAdapter(UserSingleton.Instance.Steads, this);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.ItemClick += OnSteadClick;
            UserSingleton.Instance.SteadsChanged += OnCollectionChanged;
            RegisterForContextMenu(mRecyclerView);
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            IMenuItem delete = menu.Add(Menu.None, 0, 0, "Удалить участок");
            delete.SetOnMenuItemClickListener(new MenuClick(this));
        }

        public int MenuPosition { get; set; }
        public class MenuClick : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            private SteadsActivity steadsActivity;

            public MenuClick(SteadsActivity steadsActivity)
            {
                this.steadsActivity = steadsActivity;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                switch (item.ItemId)
                {
                    case 0:
                        UserSingleton.Instance.RemoveSteadAtAsync(steadsActivity.MenuPosition);
                        return true;
                    default:
                        return false;
                }
            }
        }

        //Событие изменения коллекции участков
        public void OnCollectionChanged(object sender, EventArgs eventArgs)
        {
            mAdapter.NotifyDataSetChanged();
        }
        //Событие нажатия на участок
        private void OnSteadClick(object sender, int position)
        {
            //Toast.MakeText(this, $"This is stead {position + 1}", ToastLength.Short).Show();
            UserSingleton.Instance.CurrentStead = UserSingleton.Instance.Steads[position];
            //Переключаюсь на окно грядок
            Intent intent = new Intent(this, typeof(BedsActivity));
            StartActivity(intent);
        }

        public class SteadsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<int> ItemClick;
            public ReadOnlyCollection<Stead> Steads;

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.SteadView, parent, false);
                SteadViewHolder vh = new SteadViewHolder(itemView, OnClick, OnLongClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                SteadViewHolder vh = holder as SteadViewHolder;
                vh.Button.Text = Steads[position].Name;
            }

            public override int ItemCount
            {
                get { return Steads.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            private SteadsActivity mActivity;
            public SteadsAdapter(ReadOnlyCollection<Stead> steads, SteadsActivity activity)
            {
                Steads = steads;
                mActivity = activity;
            }
            void OnLongClick(int position)
            {
                mActivity.MenuPosition = position;
                mActivity.mRecyclerView.ShowContextMenu();
            }
        }
        public class SteadViewHolder : RecyclerView.ViewHolder
        {
            public Button Button { get; private set; }

            public SteadViewHolder(View itemView, Action<int> listener, Action<int> longListener) : base(itemView)
            {
                // Locate and cache view references:
                Button = itemView.FindViewById<Button>(Resource.Id.button);
                Button.Click += (sender, e) => listener(LayoutPosition);
                Button.LongClick += (sender, e) => longListener(LayoutPosition);
            }
        }
    }
}
using System;
using System.Collections.Generic;
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
    [Activity(Label = "Параметры грядки", Theme = "@style/AppTheme.NoActionBar")]
    public class BedActivity : AppCompatActivity
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
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FindViewById<Button>(Resource.Id.button1).Click += AddNoteAction;

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

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new NotesAdapter(bed.Notes, this);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.NoteChanged += OnNoteChanged;
            RegisterForContextMenu(mRecyclerView);
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

            return base.OnOptionsItemSelected(item);
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            IMenuItem delete = menu.Add(Menu.None, 0, 0, "Удалить записку");
            delete.SetOnMenuItemClickListener(new MenuClick(this));
        }

        public int MenuPosition { get; set; }
        public class MenuClick : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            private BedActivity bedActivity;

            public MenuClick(BedActivity bedActivity)
            {
                this.bedActivity = bedActivity;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                switch (item.ItemId)
                {
                    case 0:
                        UserSingleton.Instance.CurrentBed.DeleteNote(bedActivity.MenuPosition);
                        bedActivity.mAdapter.NotifyDataSetChanged();
                        UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
                        return true;
                    default:
                        return false;
                }
            }
        }

        private void OnNoteChanged(object sender, NotesArgs args)
        {
            UserSingleton.Instance.CurrentBed.Notes[args.Position] = args.Text;
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private NotesAdapter mAdapter;


        public void AddNoteAction(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.AddNote("");
            mAdapter.NotifyDataSetChanged();
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }

        public class NotesArgs : EventArgs
        {
            public int Position;
            public string Text;

            public NotesArgs(int pos, string text)
            {
                Position = pos;
                Text = text;
            }
        }

        public class NotesAdapter : RecyclerView.Adapter
        {
            public event EventHandler<NotesArgs> NoteChanged;
            public List<string> Notes;

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.GardenNoteView, parent, false);
                NoteViewHolder vh = new NoteViewHolder(itemView, OnNoteChanged, OnLongClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                NoteViewHolder vh = holder as NoteViewHolder;
                vh.NoteName.Text = $"Записка {position}";
                vh.Note.Text = Notes[position];
            }

            public override int ItemCount
            {
                get { return Notes.Count; }
            }

            void OnNoteChanged(int position, string text)
            {
                NoteChanged?.Invoke(this, new NotesArgs(position, text));
            }

            private BedActivity mActivity;
            public NotesAdapter(List<string> notes, BedActivity activity)
            {
                Notes = notes;
                mActivity = activity;
            }
            void OnLongClick(int position)
            {
                mActivity.MenuPosition = position;
                mActivity.mRecyclerView.ShowContextMenu();
            }
        }
        public class NoteViewHolder : RecyclerView.ViewHolder
        {
            public TextView NoteName { get; private set; }
            public EditText Note { get; private set; }

            public NoteViewHolder(View itemView, Action<int, string> listener, Action<int> longListener) : base(itemView)
            {
                // Locate and cache view references:
                NoteName = itemView.FindViewById<TextView>(Resource.Id.textView1);

                Note = itemView.FindViewById<EditText>(Resource.Id.editText1);
                Note.TextChanged += (sender, e) => listener(LayoutPosition, Note.Text);

                itemView.FindViewById<LinearLayout>(Resource.Id.linearLayout1).LongClick += (sender, e) => longListener(LayoutPosition);
            }
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
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Действие при изменении периодичности полива
        public void WaterPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(waterPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.WaterPeriod = result;
                UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
            }
        }

        //Действие при изменении даты прополки
        public void WeedDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.WeedDate = Utilities.DateTimeFormat(weedDate.Text);
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Действие при изменении периодичности прополки
        public void WeedPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(weedPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.WeedPeriod = result;
                UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
            }
        }

        //Действие при изменении даты окучивания
        public void PileUpDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.PileUpDate = Utilities.DateTimeFormat(pileUpDate.Text);
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Действие при изменении периодичности окучивания
        public void PileUpPeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(pileUpPeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.PileUpPeriod = result;
                UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
            }
        }

        //Действие при изменении даты удобрения
        public void FertilizeDateChanged(object sender, EventArgs eventArgs)
        {
            UserSingleton.Instance.CurrentBed.FertilizeDate = Utilities.DateTimeFormat(fertilizeDate.Text);
            UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
        }
        //Действие при изменении периодичности удобрения
        public void FertilizePeriodChanged(object sender, EventArgs eventArgs)
        {
            int result;
            if (int.TryParse(fertilizePeriod.Text, out result))
            {
                UserSingleton.Instance.CurrentBed.FertilizePeriod = result;
                UserSingleton.Instance.CurrentStead.InvokeBedsChanged();
            }
        }
    }
}
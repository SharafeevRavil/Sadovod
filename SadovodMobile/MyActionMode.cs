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

namespace SadovodMobile
{
    public class MyActionMode : Java.Lang.Object, ActionMode.ICallback
    {
        private Context mContext;
        public MyActionMode(Context context)
        {
            mContext = context;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.itemOneId:
                    // do whatever you want
                    return true;
                default:
                    return false;
            }
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.ContextualMenu, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            mode.Dispose();
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }
}
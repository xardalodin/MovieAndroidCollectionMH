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

namespace SlidingTabLayout
{
    class Movie_ListView_Adapter : BaseAdapter<backend.movie>
    {
        private List<backend.movie> mItems;
        private Context mContext;


        public Movie_ListView_Adapter(Context context, IEnumerable<backend.movie> items)
        {
            mItems = items.ToList();
            mContext = context;
        }

        public override int Count
        {
            get { return mItems.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override backend.movie this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Movie_ListView_Row, null, false);
            }

            TextView txtMovie = row.FindViewById<TextView>(Resource.Id.txtmovie);
            txtMovie.Text = mItems[position].Movie;

            TextView txtLenght = row.FindViewById<TextView>(Resource.Id.txtlenght);
            txtLenght.Text = mItems[position].Length;

            TextView txtFormat = row.FindViewById<TextView>(Resource.Id.txtformat);
            txtFormat.Text = mItems[position].Format;

            return row;
        }
    }
}
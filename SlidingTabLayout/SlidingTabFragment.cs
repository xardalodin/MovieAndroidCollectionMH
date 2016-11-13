using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Java.Lang;
using SlidingTabLayout.backend;

namespace SlidingTabLayout
{
    public class SlidingTabFragment : Fragment
    {
        private SlidingTabScrollView mSlidingTabScrollView;
        private ViewPager mViewPager;

       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sapmle, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);

            mViewPager.Adapter = new SamplePagerAdapter();

            mSlidingTabScrollView.ViewPager = mViewPager;
        }



        public class SamplePagerAdapter : PagerAdapter
        {
            List<string> items = new List<string>();
            private TextView TxtServerId;

            public SamplePagerAdapter() : base()
            {
                items.Add("Movies owned");
                items.Add("Movies to Buy");
                items.Add("Load From Computer");
               // items.Add("Hooray");

            }

            public override int Count
            {
                get
                {
                    return items.Count;
                }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
            {
                return view == objectValue;
            }


            // this  InstantiateItem depending on the position it desides witch layout to use.
            // a case statement on the poition and just load the layout to the view.     

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                //position 1 Movies owned layout axml ListView movie lenght format, Load from json file..
                if (position == 0)
                {
                    View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.MovieMainList, container, false);
                    container.AddView(view);

                    ListView mMovieList = view.FindViewById<ListView>(Resource.Id.mylistviewmovie);
                    List<backend.movie> mov = new List<backend.movie>();
                    backend.Movie_Json_Services service = new backend.Movie_Json_Services();
                    mov = service.GetMovieLocalCopy().ToList();

                    Movie_ListView_Adapter adapter = new Movie_ListView_Adapter(view.Context, mov);
                    mMovieList.Adapter = adapter;
                    return view;

                } else if(position == 2)
                {

                    View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.layout_Server_config, container, false);
                    container.AddView(view);

                    TxtServerId =(EditText)view.FindViewById(Resource.Id.txtServerId);
                    Button BtnServer = view.FindViewById<Button>(Resource.Id.btnServer);

                    BtnServer.Click += BtnServer_Click;

                    return view;

                }
                else
                {
                    //position 2 Movies To buy layout axml = movie,lenght,format  Load form jsonfile

                    //position 3 a EditText and a button will do and a TextView with instructions.

                    View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
                    container.AddView(view);

                    TextView txtTitle = view.FindViewById<TextView>(Resource.Id.item_title);
                    int pos = position + 1;
                    txtTitle.Text = pos.ToString();

                    return view;
                }
            }

            private async void BtnServer_Click(object sender, EventArgs e)
            {
                string uribuild = "http://"+TxtServerId.Text+"/simpleserver/";
                Uri uri = new Uri(uribuild);
                backend.Movie_Json_Services service = new backend.Movie_Json_Services();
                IEnumerable<backend.movie> mov = await service.GetMoviesServer(uri);
                
            }

            public string GetHeaderTitle(int posoition)
            {
                return items[posoition];
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
            {
                container.RemoveView((View)objectValue);
            }
        }
    }
}
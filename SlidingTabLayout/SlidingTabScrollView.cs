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
using Android.Support.V4.View;
using Android.Util;

namespace SlidingTabLayout
{
    public class SlidingTabScrollView: HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_PADDING_DIPS = 16;
        private const int TAB_VIEW_TEXT_SIZE_SP = 12;

        private int mTitleOffset;

        private int mTabViewLayoutID;
        private int mTabViewTextID;

        private ViewPager mviewPager;
        private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

        private static SlidingTabStrip mTabStrip;

        private int mScrollsState;

        public interface TabColorizer
        {
            int GetInicatorColor(int postion);
            int GetDividerColor(int position);

        }

        public SlidingTabScrollView(Context context) : this(context, null) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs, int defaultStyle) : base(context, attrs, defaultStyle)
        {
            // disable the scrollbar
            HorizontalScrollBarEnabled = false;

            //make sure the tab 
            FillViewport = true;
            this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xE5, 0xE5, 0xE5));

            mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

            mTabStrip = new SlidingTabStrip(context);
            this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }

        public TabColorizer CustomTabColorizer
        {
            set { mTabStrip.CustomTabColorizer = value; }
        }

        public int [] SelectedIndicator
        {
            set { mTabStrip.SelectedIndicatorColors = value; }
        }

        public int [] DividerColors
        {
            set { mTabStrip.DividerColors = value; }
        }

        public ViewPager.IOnPageChangeListener onPageListener
        {
            set { mViewPagerPageChangeListener = value; }
        }

        public ViewPager ViewPager
        {
            set
            {
                mTabStrip.RemoveAllViews();

                mviewPager = value;
                if (value != null)
                {
                    value.PageSelected += Value_PageSelected;
                    value.PageScrollStateChanged += value_PageScrollStateChanged;
                    value.PageScrolled += value_PageScrolled;
                    PopulateTabStrip();
                }

            }

        }

        private void value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            int TabCount = mTabStrip.ChildCount;

            if ((TabCount == 0) || (e.Position < 0) || (e.Position >= TabCount))
            {
                //if any of these considtions apply, return, no need to scroll
                return;
            }
            mTabStrip.OnViewPagerPageChamnged(e.Position, e.PositionOffset);

            View seletedTitle = mTabStrip.GetChildAt(e.Position);

            int extraOffset = (seletedTitle != null ? (int)(e.Position *seletedTitle.Width): 0);

            ScrollToTab(e.Position, extraOffset);

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrolled(e.Position, e.PositionOffset, e.PositionOffsetPixels);
            }

        }

        private void PopulateTabStrip()
        {
            PagerAdapter adapter = mviewPager.Adapter;

            for (int i = 0; i < adapter.Count; i++)
            {
                TextView tabView = CreateDefaultTabView(Context);
                tabView.Text = ((SlidingTabFragment.SamplePagerAdapter)adapter).GetHeaderTitle(i);
                //tabView.Text = "dude";
                tabView.SetTextColor(Android.Graphics.Color.Black);
                tabView.Tag = i;
                tabView.Click += tabView_Click;
                mTabStrip.AddView(tabView);
            }

        }

        private TextView CreateDefaultTabView(Context context)
        {

            TextView textView = new TextView(context);
            textView.Gravity = GravityFlags.Center;
            textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
            textView.Typeface = Android.Graphics.Typeface.DefaultBold;

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb)
            {
                TypedValue outvalue = new TypedValue();
                Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outvalue, false);
                textView.SetBackgroundResource(outvalue.ResourceId);

            }

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.IceCreamSandwich)
            {
                textView.SetAllCaps(true);
            }
            int padding = (int)(TAB_VIEW_PADDING_DIPS * Resources.DisplayMetrics.Density);
            textView.SetPadding(padding, padding, padding, padding);

            return textView;
        }

        private void tabView_Click(object sender, EventArgs e)
        {
            TextView clickTab = (TextView)sender;
            int pageToScrollto = (int)clickTab.Tag;
            mviewPager.CurrentItem = pageToScrollto;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (mviewPager != null)
            {
                ScrollToTab(mviewPager.CurrentItem, 0);
            }
        }


        private void ScrollToTab(int tabIndex, int extraOffset)
        {
            int tabCount = mTabStrip.ChildCount;

            if (tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
            {
                //No need to go futher ,dont scroll
                return;
            }

            View selectedChiled = mTabStrip.GetChildAt(tabIndex);
            if (selectedChiled != null)
            {
                int scrollAmountX = selectedChiled.Left + extraOffset;

                if (tabIndex > 0 || extraOffset > 0)
                {
                    scrollAmountX -= mTitleOffset;
                }
                this.ScrollTo(scrollAmountX, 0);
            }
                
        }

        private void value_PageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            mScrollsState = e.State;

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);

            }
        }

        private void Value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (mScrollsState == ViewPager.ScrollStateIdle)
            {
                mTabStrip.OnViewPagerPageChamnged(e.Position, 0f);
                ScrollToTab(e.Position, 0);
            }

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageSelected(e.Position);

            }
        }
    }
}
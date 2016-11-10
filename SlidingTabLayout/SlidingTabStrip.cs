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
using Android.Graphics;
using Android.Util;

namespace SlidingTabLayout
{
   public class SlidingTabStrip : LinearLayout
    {
        private const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
        private const byte DEFAULT_BOTTOM_BORDER_COLOR_APLHA = 0X26;
        private const int SELECTED_INDICATOR_THICKNESS_DIPS = 8;
        private int[] INDICATOR_COLORS = { 0x19319, 0x0000FC,0x00000 };
        private int[] DIVEDER_COLORS = { 0xC5C5C5 };

        private const int DEFAULT_DIVIDER_THICKNESS_DIPS = 1;
        private const float DEFAULT_DIVIDER_HEIGHT = 0.5f;

        //bottom border
        private int mBottomBorderthickness;
        private Paint mBottomBorderPaint;
        private int mDefaultBottomBorderColor;

        //indicator
        private int mSelectedIndicatorThickness;
        private Paint mSelectedIndicatorPaint;

        //divider
        private Paint mDividerPaint;
        private float mDividerHeight;

        //selector position and offset
        private int mSelectorPosition;
        private float mSelectionOffset;

        //Tab colorizer
        private SlidingTabScrollView.TabColorizer mCustomTabColorizer;
        private SimpleTabColorizer mDefaultTabColorizer;


        //Constructor
        public SlidingTabStrip(Context context) : this(context, null)
        { }

        public SlidingTabStrip(Context context,IAttributeSet attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);

            float density = Resources.DisplayMetrics.Density;
            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outValue, true);
            int themeForeGround = outValue.Data;
            mDefaultBottomBorderColor = SetColorAlpha(themeForeGround, DEFAULT_BOTTOM_BORDER_COLOR_APLHA);

            mDefaultTabColorizer = new SimpleTabColorizer();
            mDefaultTabColorizer.IndicatorColors = INDICATOR_COLORS;
            mDefaultTabColorizer.DividerColor = DIVEDER_COLORS;

            mBottomBorderthickness = (int)(DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS * density);
            mBottomBorderPaint = new Paint();
            mBottomBorderPaint.Color = GetColorFromInteger(0xC5C5C5); //gray

            mSelectedIndicatorThickness = (int)(SELECTED_INDICATOR_THICKNESS_DIPS * density);
            mSelectedIndicatorPaint = new Paint();

            mDividerHeight = DEFAULT_DIVIDER_HEIGHT;
            mDividerPaint = new Paint();
            mDividerPaint.StrokeWidth = (int)(DEFAULT_DIVIDER_THICKNESS_DIPS * density);

        }

        public SlidingTabScrollView.TabColorizer CustomTabColorizer
        {
            set { mCustomTabColorizer = value;
                this.Invalidate();  }
        }

        public int[] SelectedIndicatorColors
        {
            set
            {
                mCustomTabColorizer = null;
                mDefaultTabColorizer.IndicatorColors = value;
                this.Invalidate();
            }

        }

        public int[] DividerColors
        {
            set
            {
                mDefaultTabColorizer = null;
                mDefaultTabColorizer.DividerColor = value;
                this.Invalidate();
            }

        }


        private Color GetColorFromInteger(int v)
        {

            return Color.Rgb(Color.GetRedComponent(v), Color.GetGreenComponent(v), Color.GetBlueComponent(v));
        }

        private int SetColorAlpha(int v, byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(v), Color.GetGreenComponent(v), Color.GetBlueComponent(v));
        }

        public void OnViewPagerPageChamnged(int position, float positionOffset)
        {
            mSelectorPosition = position;
            mSelectionOffset = positionOffset;
            this.Invalidate();

        }
        protected override void OnDraw(Canvas canvas)
        {
            int height = Height;
            int tabCount = ChildCount;
            int dividerHeightPx = (int)(Math.Min(Math.Max(0f,mDividerHeight),1f)*height);
            SlidingTabScrollView.TabColorizer tabcolorizer = mCustomTabColorizer != null ? mCustomTabColorizer : mDefaultTabColorizer;
            //thock colored underline below the current selection 
            if (tabCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectorPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabcolorizer.GetInicatorColor(mSelectorPosition);

                if (mSelectionOffset > 0f && mSelectorPosition < (tabCount - 1))
                {
                    int nextColor = tabcolorizer.GetInicatorColor(mSelectorPosition + 1);
                    if (color != nextColor)
                    {
                        color = blendColor(nextColor, color, mSelectionOffset);
                    }
                    View nextTitle = GetChildAt(mSelectorPosition + 1);
                    left = (int)(mSelectionOffset * nextTitle.Left + (1.0f - mSelectionOffset) * left);
                    right = (int)(mSelectionOffset * nextTitle.Right + (1.0f - mSelectionOffset) * right);


                }

                mSelectedIndicatorPaint.Color = GetColorFromInteger(color);
                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);

                //create vertical dividers between tabs
                int separatorTop = (height - dividerHeightPx) / 2;
                for (int i = 0; i < ChildCount; i++)
                {
                    View child = GetChildAt(i);
                    mDividerPaint.Color = GetColorFromInteger(tabcolorizer.GetDividerColor(i));
                    canvas.DrawLine(child.Right, separatorTop, child.Right, separatorTop + dividerHeightPx, mDividerPaint);
                }

                canvas.DrawRect(0, height - mBottomBorderthickness, Width, height, mBottomBorderPaint);
            }

        }

        private int blendColor(int Color1, int Color2, float ratio)
        {
            float inverseRatio = 1f - ratio;
            float r = (Color.GetRedComponent(Color1) * ratio) + (Color.GetRedComponent(Color2) * inverseRatio);
            float b = (Color.GetBlueComponent(Color1) * ratio) + (Color.GetBlueComponent(Color2) * inverseRatio);
            float g = (Color.GetGreenComponent(Color1) * ratio) + (Color.GetGreenComponent(Color2) * inverseRatio);

            return Color.Rgb((int)r, (int)g, (int)b);
        }

        private class SimpleTabColorizer : SlidingTabScrollView.TabColorizer
        {
            private int[] mIndicatorColor;
            private int[] mDividerColors;

            public int GetInicatorColor(int position)
            {
                return mIndicatorColor[position % mIndicatorColor.Length];
            }

            public int GetDividerColor(int position)
            {
                return mDividerColors[position % mDividerColors.Length];
            }

            public int[] IndicatorColors
            {
                set { mIndicatorColor = value; }
            }

            public int[] DividerColor
            {
                set { mDividerColors = value; }

            }
        }

    }
}
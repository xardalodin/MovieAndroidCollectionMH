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

namespace SlidingTabLayout.backend
{
    public class movie
    {
        private string mov;
        private string length;
        private string format;
        // add imdb LINK so can be open if you click it in listview.

        public string Movie { get { return mov; } set { mov = value; } }

        public string Length { get { return length; } set { length = value; } }

        public string Format { get { return format; } set { format = value; } }

        public movie()   //just an emty one for the json crap 
        {
            //do nothing
        }
        public movie(string movie, string length, string format)
        {
            this.mov = movie;
            this.length = length;
            this.format = format;
        }
    }
}
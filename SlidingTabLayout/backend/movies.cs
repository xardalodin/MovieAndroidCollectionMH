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
     public class movies
    {
        List<movie> listOfMovie;
        public  List<backend.movie> ListOfMovies { get { return listOfMovie; } set { listOfMovie = value; } }
     }
}
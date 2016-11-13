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
using Newtonsoft.Json;

namespace SlidingTabLayout.backend
{
    public class DecodeJson
    {
        public static IEnumerable<backend.movie> DecodeJsonString(string json)
        {
            try
            {
                backend.movies mov = JsonConvert.DeserializeObject<backend.movies>(json);
                return mov.ListOfMovies;
            }
            catch
            {
                backend.movies mov2 = new backend.movies();
                backend.movie error = new backend.movie() { Movie = "error", Length = "wrong", Format = "Format" };
                List<movie> listOfMovie = new List<movie>();
                listOfMovie.Add(error);
                return listOfMovie;
            }
        }

    }
}
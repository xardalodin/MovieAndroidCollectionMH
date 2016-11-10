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
            backend.movies mov = JsonConvert.DeserializeObject<backend.movies>(json);
            return mov.ListOfMovies;
        }

    }
}
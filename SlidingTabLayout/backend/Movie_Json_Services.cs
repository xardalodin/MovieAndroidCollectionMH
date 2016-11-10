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
using System.Net;
using System.Threading.Tasks;
using Android.Support.V4.Content;
using System.IO;
using System.ComponentModel;

namespace SlidingTabLayout.backend
{
    public class Movie_Json_Services
    {
        private Uri urimain;

        public Movie_Json_Services()
        {
            /*
            Uri uri = new Uri("http://www.contribe.se/arbetsprov-net/books.json");
            Movie_Json_Services  m = new Movie_Json_Services();
            Task<string> download =  await m.downloadjson(uri);  //setup
            string json = await download;
            m.writetofile(json);   
           */
        }

        public string PathToFile(string filename)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            return path;
        }

        public string ReadFromFile()
        {
          //  File.Delete(PathToFile("json.json")); // delete it 
            return File.ReadAllText(PathToFile("json.json"));
        }

        public void writetofile(string jsondata)  // store a local compy of movies
        {
            bool exists = File.Exists(PathToFile("json.json"));
            if (!exists)// does not exists
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, "json.json");
                System.IO.File.WriteAllText(filePath, jsondata);
            }
            else
            {
                File.Delete(PathToFile("json.json")); // delete it 
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // then create it
                var filePath = Path.Combine(documentsPath, "json.json");
                System.IO.File.WriteAllText(filePath, jsondata);
            }

        }

        public IEnumerable<movie> GetMovieLocalCopy()
        {
            bool exists = File.Exists(PathToFile("json.json"));
            if (!exists)// does not exists
            {
                List<backend.movie> mov = new List<backend.movie>();
                mov.Add(new backend.movie() { Movie = "hello", Length = "10min", Format = "DVD" });
                return mov;
            }
            else
            {
                return DecodeJson.DecodeJsonString(ReadFromFile());
            }

        }



        public async Task<IEnumerable<movie>> GetMoviesServer(Uri uri)
        {
            try
            {
                urimain = uri;
                Task<string> download = Downloadjson();
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("before testing: " + uri);
                
                string json = await download;
                writetofile(json);                 // Write to file on Phone.
                Console.WriteLine("after testing : " + json);
               return DecodeJson.DecodeJsonString(json);
            }
            catch (Exception x)
            {
                Console.WriteLine("fuck\n");
                return null;

            }
        }

        public async Task<string> Downloadjson()
        {   try
            {
                var client = new WebClient();
                string json = await client.DownloadStringTaskAsync(urimain);
                return json;
            }
            catch (WebException x)
            {
                Console.WriteLine("\n error :" + x);
                return  "hello";
            }
        }


    }
}
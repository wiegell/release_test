using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Threading.Tasks;
using SkiaSharp;

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AstroWall
{
    public class FileHelpers
    {
        public FileHelpers()
        {
        }

        public static string getImageStoreDirectory()
        {
            return MacOShelpers.getAstroDirectory() + Guid.NewGuid();
        }

        public static async Task<String> DownloadUrlToTmpPath(string imgurl)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(imgurl);
            string ext = System.IO.Path.GetExtension(imgurl);

            string localFileName = getImageStoreDirectory() + ext;
            Console.WriteLine("Downloading file: " + imgurl);
            Console.WriteLine("Writing to tmp path: " + localFileName);
            Task t = client.DownloadFileTaskAsync(uri, localFileName);
            await t;
            Console.WriteLine("Write complete");

            return localFileName;
        }

        public async static Task<SKBitmap> LoadImageFromLocalUrl(string path)
        {

            SKBitmap bitmap;
            try
            {

                using (MemoryStream memStream = new MemoryStream())
                {
                    FileStream fs = new FileStream(path, FileMode.Open);

                    await fs.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    SKImage img = SKImage.FromEncodedData(memStream);
                    bitmap = SKBitmap.FromImage(img);
                    memStream.Seek(0, SeekOrigin.Begin);

                };

            }

            catch (Exception ex)
            {
                Console.WriteLine("image load error: " + path + ", " + ex.Message);
                throw ex;
            }
            return bitmap;

        }

        public static void SerializeNow(Object c, string path)
        {
            string jsonString = JsonConvert.SerializeObject(c);
            File.WriteAllText(path, jsonString);
        }

        //public static List<T> DeSerializeNow<T>(string path)
        //{

        //    string jsonString = String.Join("", File.ReadAllLines(path));
        //    //dynamic json = JsonConvert.DeserializeObject(jsonString);
        //    List<T> result = JsonConvert.DeserializeObject<List<T>>(jsonString);
        //    return result;
        //}

        public static T DeSerializeNow<T>(string path)
        {

            string jsonString = String.Join("", File.ReadAllLines(path));
            //dynamic json = JsonConvert.DeserializeObject(jsonString);
            T result = JsonConvert.DeserializeObject<T>(jsonString);
            return result;
        }

        public static bool DBExists()
        {
            return File.Exists(MacOShelpers.getDBPath());
        }

        public static bool PrefsExists()
        {
            return File.Exists(MacOShelpers.getPrefsPath());
        }

    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Library
{
    public class Database : Server
    {
        private bool _connected;

        public static async void SaveAsync<T>(T obj)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\users.json", FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
            stream.Close();
        }

        public static async void SaveAsync<T>(string path, T obj)
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
            stream.Close();
        }

        public static string Load()
        {
            return File.ReadAllText(Directory.GetCurrentDirectory() + @"\users.json", Encoding.ASCII);
        }

        public static async void SaveAsync<T>(T obj, string filename)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"/" + filename, FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
            stream.Close();
        }

        public static void Save<T>(T obj)
        {
            //File.WriteAllText(Directory.GetCurrentDirectory() + @"\users.json", string.Empty);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\users.json", JsonConvert.SerializeObject(obj), Encoding.ASCII);

            //FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\users.json", FileMode.OpenOrCreate);
            //StreamWriter writer = new StreamWriter(stream);
            //Console.WriteLine(JsonConvert.SerializeObject(obj));
            //string a = JsonConvert.SerializeObject(obj);
            //stream.
            //writer.Write(a);
            //writer.Close();
            //stream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj).ToCharArray()), 0, (int)stream.Length);
            //stream.Close();
        }

        public static void Save<T>(string path, T obj)
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            stream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj).ToCharArray()), 0, (int)stream.Length);

            stream.Close();
        }

        public static void Save<T>(T obj, string filename)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"/" + filename, FileMode.OpenOrCreate);
            stream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj)), 0, (int)stream.Length);

            stream.Close();
        }

        public override bool Connect()
        {
            _connected = true;
            return true;
        }
    }
}
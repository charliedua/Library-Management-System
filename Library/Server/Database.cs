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
        public static async void SaveAsync<T>(T obj)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\users.json", FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }

        public static async void SaveAsync<T>(string path, T obj)
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }

        public static async void SaveAsync<T>(T obj, string filename)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"/" + filename, FileMode.OpenOrCreate);
            await stream.WriteAsync(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }

        public static void Save<T>(T obj)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\users.json", FileMode.OpenOrCreate);
            stream.Write(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }

        public static void Save<T>(string path, T obj)
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            stream.Write(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }

        public static void Save<T>(T obj, string filename)
        {
            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"/" + filename, FileMode.OpenOrCreate);
            stream.Write(new byte[stream.Length], 0, (int)stream.Length);
            JsonConvert.SerializeObject(obj);
        }
    }
}
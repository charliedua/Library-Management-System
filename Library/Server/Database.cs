using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library
{
    /// <summary>
    /// methods for a file db maintained by json
    /// </summary>
    /// <seealso cref="Library.Server" />
    public class Database : Server
    {
        /// <summary>
        /// The status of server.
        /// </summary>
        private bool _connected;

        /// <summary>
        /// Loads the Object of Type T async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\users.json", Encoding.ASCII));
        }

        public static T Load<T>(string filename)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\" + filename, Encoding.ASCII));
        }

        /// <summary>
        /// Loads the Object of Type T async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(string path)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path, Encoding.ASCII));
            });
        }

        /// <summary>
        /// Loads the Object of Type T async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">The filename.</param>
        /// <param name="temp">The temporary.</param>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(string filename, int temp = 0)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\" + filename, Encoding.ASCII));
            });
        }

        /// <summary>
        /// Loads the Object of Type T async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<T> LoadAsync<T>()
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\users.json", Encoding.ASCII));
            });
        }

        /// <summary>
        /// Saves the specified object to users.json in CWD.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        public static void Save<T>(T obj)
        {
            string path = Directory.GetCurrentDirectory() + @"\users.json";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
        }

        /// <summary>
        /// Saves the object to the specified path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object.</param>
        public static void Save<T>(string path, T obj)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
        }

        /// <summary>
        /// Saves the specified object to the filename specified in CWD.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="filename">The filename.</param>
        public static void Save<T>(T obj, string filename)
        {
            string path = Directory.GetCurrentDirectory() + @"\" + filename;
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
        }

        /// <summary>
        /// Saves the specified object to users.json in CWD async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        public static async void SaveAsync<T>(T obj)
        {
            await Task.Run(() =>
            {
                string path = Directory.GetCurrentDirectory() + @"\users.json";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
            });
        }

        /// <summary>
        /// Saves the specified object to the path specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object.</param>
        public static async void SaveAsync<T>(string path, T obj)
        {
            await Task.Run(() =>
             {
                 if (!File.Exists(path))
                 {
                     File.Create(path);
                 }
                 File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
             });
        }

        /// <summary>
        /// Saves the Object to file async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="filename">The filename.</param>
        public static async void SaveAsync<T>(T obj, string filename)
        {
            await Task.Run(() =>
            {
                string path = Directory.GetCurrentDirectory() + @"\" + filename;
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                File.WriteAllText(path, JsonConvert.SerializeObject(obj), Encoding.ASCII);
            });
        }

        /// <summary>
        /// Connects to server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        public override bool Connect()
        {
            _connected = true;
            return _connected;
        }
    }
}
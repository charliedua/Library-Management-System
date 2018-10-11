using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Library.Utils
{
    /// <summary>
    /// Contains utility methods
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Reads the password from console in a secure way.
        /// </summary>
        /// <returns></returns>
        public static string ReadPassword()
        {
            string password = "";
            for (ConsoleKeyInfo info = Console.ReadKey(true); info.Key != ConsoleKey.Enter; info = Console.ReadKey(true))
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                    }
                }
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        /// <summary>
        /// Gives Human readable permissions.
        /// </summary>
        /// <returns></returns>
        public static string HumanReadablePermissions(List<Permissions> permissions)
        {
            string final = "";
            foreach (Permissions permission in permissions)
            {
                final += "\t" + permission.ToString() + " \n";
            }
            return final;
        }

        /// <summary>
        /// converts Int to permissions.
        /// </summary>
        /// <param name="num">The number.</param>
        public static List<Permissions> IntToPerm(int num)
        {
            List<Permissions> permissions = new List<Permissions>();

            switch (num)
            {
                // CRD
                // 000
                case 0:
                    permissions.Add(Permissions.None);
                    break;

                // CRD
                // 001
                case 1:
                    permissions.Add(Permissions.Delete);
                    break;

                // CRD
                // 010
                case 2:
                    permissions.Add(Permissions.Read);
                    break;

                // CRD
                // 011
                case 3:
                    permissions.Add(Permissions.Read);
                    goto case 1;

                // CRD
                // 100
                case 4:
                    permissions.Add(Permissions.Create);
                    break;

                // CRD
                // 101
                case 5:
                    permissions.Add(Permissions.Delete);
                    goto case 4;

                // CRD
                // 110
                case 6:
                    permissions.Add(Permissions.Create);
                    goto case 2;

                // CRD
                // 111
                case 7:
                    permissions.Add(Permissions.Create);
                    goto case 3;

                default:
                    return null;
            }
            return permissions;
        }

        /// <summary>
        /// Converts Permissions to int.
        /// </summary>
        public static int PermToInt(List<Permissions> permissions)
        {
            int final = 0;
            foreach (var perm in permissions)
            {
                final += (int)perm;
            }
            return final;
        }

        public static List<User> LoadAllUsers()
        {
            using (IDbConnection db = new SQLiteConnection("Data Source=.//Library.db;Version=3;"))
            {
                return db.Query<User>
                ("Select * From Users").ToList();
            }
        }

        public static void LoadInventory(List<User> users, List<LibraryItem> items)
        {
            using (IDbConnection db = new SQLiteConnection("Data Source=.//Library.db;Version=3;"))
            {
                IEnumerable<dynamic> data = db.Query("SELECT UserID, ItemID, IssuedOn FROM Issues JOIN Items JOIN Users WHERE ItemID = Items.ID AND UserID = Users.ID");
                foreach (dynamic item in data)
                {
                    int ItemID = (int)item.ItemID;
                    int UserID = (int)item.UserID;
                    DateTime? dateTime = item.IssuedOn;
                    users.Find(x => x.ID == UserID).Inventory.Put(items.Find(x => x.ID == ItemID));
                }
            }
        }

        public static List<LibraryItem> LoadAllItems()
        {
            using (IDbConnection db = new SQLiteConnection("Data Source=.//Library.db;Version=3;"))
            {
                return db.Query<LibraryItem>("SELECT * FROM Items").ToList();
            }
        }

        public static void SaveAllItems(List<LibraryItem> items)
        {
            string sql = "INSERT INTO Items (ID, Name, Available, MaximumLoanPeriod) VALUES (@ID, @Name, @Available, @MaximumLoanPeriod)";
            using (IDbConnection db = new SQLiteConnection("Data Source=.//Library.db;Version=3;"))
            {
                db.Open();
                int affectedRows = db.Execute(sql, items);
            }
        }

        public static void SaveAllUsers(List<User> users)
        {
            string sql = "INSERT INTO Users (ID, Name, Permissions, State, MaximumItems, Username, Password) VALUES (@ID, @Name, @Permissions, @State, @MaximumItems, @Username, @Password)";
            using (IDbConnection db = new SQLiteConnection("Data Source=.//Library.db;Version=3;"))
            {
                foreach (var user in users)
                {
                    long perms = PermToInt(user.Permissions);
                    long ID = user.ID;
                    if (user.HasAccount)
                    {
                        long State = (int)user.Account.State;
                        db.Query(sql, new { ID, user.Name, Permissions = perms, State, MaximumItems = user.MaxItems, user.Account.Username, user.Account.Password });
                    }
                    else
                    {
                        db.Query(sql, new { ID, user.Name, Permissions = perms, State = (long?)null, MaximumItems = user.MaxItems, Username = (string)null, Password = (string)null });
                    }
                }
            }
        }
    }
}
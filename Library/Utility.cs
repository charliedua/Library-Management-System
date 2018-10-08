using System;
using System.Collections.Generic;

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
    }
}
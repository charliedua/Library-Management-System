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
    }
}
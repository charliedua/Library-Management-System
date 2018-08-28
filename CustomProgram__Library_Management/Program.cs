using Library;
using System;

namespace CustomProgram__Library_Management
{
    /// <summary>
    /// The main program class for the entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            User user = new User("Charlie", "101983924");
            user.CreateAccount("new account");
            user.Save();
            user = null;
            Console.WriteLine(user.Identifier);
            Console.WriteLine(user.Name);
            Console.WriteLine(user.Account.Password);
            Console.WriteLine("Finished!");
        }
    }
}
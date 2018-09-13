using Library;
using Library.Commands;
using Library.Utils;
using System;
using System.Security;

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
            Console.Write("Login as: ");
            string username = Console.ReadLine();
            Console.Write($"Enter Password for {username}@Library: ");
            string password = Utility.ReadPassword();
            Database database = new Database();
            var reader = database.LoadReader("Users", $"Username = '{username}'");
            User user = User.Load(reader);
            reader.Close();
            database.Dispose();
            if (user == null)
            {
                Console.WriteLine("Not Found.");
            }
            else
            {
                Console.WriteLine(user.Details);
            }

            Command command = new CreateCommand();
            while (true)
            {
                Console.Write("Write your command here $ ");
                var text = Console.ReadLine();
                Console.WriteLine(command.Execute(ref entity, text.Split(' ')));
            }
        }
    }
}
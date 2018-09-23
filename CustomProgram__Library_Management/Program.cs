using Library;
using Library.Commands;
using Library.Utils;
using System;
using System.Data.SQLite;
using System.Security;
using System.Text.RegularExpressions;

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
            LibraryController controller = new LibraryController();
            controller.LoadAllEntities();
            string password;
            string username;
            string helpText = "";
            bool verified;
            do
            {
                do
                {
                    Console.Write("Login as: ");
                    username = Console.ReadLine();
                    helpText = Regex.IsMatch(username, @"^[A-Za-z0-9]+$") ? "" : "Wrong Format for username\n";
                    Console.Write(helpText);
                } while (!Regex.IsMatch(username, @"^[A-Za-z0-9]+$"));
                Console.Write($"Enter Password for {username}@Library: ");
                password = Utility.ReadPassword();
                verified = controller.Login(username, password);
                helpText = !verified ? "Wrong Credentials\n" : "Successfully logged in\n";
                Console.Write(helpText);
            } while (!verified);
            CommandProcessor processor = new CommandProcessor(controller, Quit);
            string text = "";
            while (true)
            {
                Console.Write("Write your command here $ ");
                text = Console.ReadLine();
                Console.WriteLine(processor.Invoke(text));
            }
        }

        private static void Quit()
        {
            System.Environment.Exit(Environment.ExitCode);
        }
    }
}
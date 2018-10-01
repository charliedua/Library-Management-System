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
            CommandProcessor processor = new CommandProcessor(controller, Quit, Clear);
            string userInput = "";
            controller.LoadAllEntities();
            AskForLogin(controller);
            while (true)
            {
                while (controller._authenticated)
                {
                    Console.Write("Write your command here $ ");
                    userInput = Console.ReadLine();
                    Console.WriteLine(processor.Invoke(userInput));
                }
                AskForLogin(controller);
            }
        }

        private static void Quit()
        {
            Environment.Exit(Environment.ExitCode);
        }

        private static void Clear()
        {
            Console.Clear();
        }

        private static void AskForLogin(LibraryController controller)
        {
            string password;
            string username;
            string helpText = "";
            bool verified;
            do
            {
                do
                {
                    Console.Clear();
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
        }
    }
}
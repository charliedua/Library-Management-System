using Library;
using Library.Commands;
using Library.Utils;
using System;
using System.Collections.Generic;
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
            CommandProcessor processor = new CommandProcessor(controller, Quit, Clear, AskFunc, EditFunc);
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

        private static void EditFunc(Entity entity, Entities type)
        {
            if (type == Entities.User && entity is User user)
            {
                List<string> options = new List<string>() { "Name", "Username", "Password", "Permissions" };
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ". " + options[i]);
                }
                Console.WriteLine("What do you want to edit: ");
                if (!int.TryParse(Console.ReadLine(), out int option) && options.Count <= option)
                {
                    Console.WriteLine("Error in input, Please Try again");
                }
                Console.Write("please Enter new " + options[option - 1] + " : ");
                switch (option)
                {
                    case 1:
                        user.Name = Console.ReadLine();
                        break;

                    case 2:
                        if (user.HasAccount)
                        {
                            if (!user.Account.SetUsername(Console.ReadLine()))
                                Console.WriteLine("Somneone has a same username as you.");
                            else
                                Console.Write("Worked");
                        }
                        else
                        {
                            Console.WriteLine("You don't have an account");
                        }
                        break;

                    case 3:
                        user.Account.SetPassword(Utility.ReadPassword());
                        break;

                    case 4:
                        if (int.TryParse(Console.ReadLine(), out int perms))
                        {
                            user.Permissions = Utility.IntToPerm(perms);
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    default:
                        Console.Write("Error in Input");
                        break;
                }
            }
            else if (type == Entities.Item && entity is LibraryItem item)
            {
                List<string> options = new List<string>() { "Name" };
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ". " + options[i]);
                }
                Console.WriteLine("What do you want to edit: ");
                if (!int.TryParse(Console.ReadLine(), out int option) && options.Count <= option)
                {
                    Console.WriteLine("Error in input, Please Try again");
                }
                Console.WriteLine("please Enter new " + options[option - 1] + " : ");
                switch (option)
                {
                    case 1:
                        item.Name = Console.ReadLine();
                        break;

                    default:
                        Console.Write("Error in Input");
                        break;
                }
            }
        }

        private static void Quit()
        {
            Environment.Exit(Environment.ExitCode);
        }

        private static string AskFunc(string toPrint)
        {
            Console.Write(toPrint);
            return Utility.ReadPassword();
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
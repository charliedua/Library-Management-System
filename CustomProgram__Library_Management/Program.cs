using Library;
using Library.Commands;
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
            Entity entity = null;
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
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
            LibraryItem item = new LibraryItem("Katie");
            item.Save();
            item = null;
            item = new LibraryItem("new");
            Console.WriteLine(item.Name);
            Console.WriteLine(item.ID);
            Console.ReadLine();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Computer : LibraryItem
    {
        /// <summary>
        /// The maximum booking time for computer in hrs
        /// </summary>
        public const int MAX_BOOKING_TIME = 2;

        public Computer(string name, string identifier) : base(name, identifier)
        {
        }

        public bool Give()
        {
            throw new NotImplementedException();
        }

        public bool Take()
        {
            throw new NotImplementedException();
        }
    }
}
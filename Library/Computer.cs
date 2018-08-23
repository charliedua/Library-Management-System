using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Computer : IssuableItem, IIssuable
    {
        /// <summary>
        /// The maximum booking time for computer in hrs
        /// </summary>
        public const int MAX_BOOKING_TIME = 2;

        public Computer(string name, string identifier) : base(name, identifier)
        {
        }

        public override bool Give()
        {
            throw new NotImplementedException();
        }

        public override bool Take()
        {
            throw new NotImplementedException();
        }
    }
}
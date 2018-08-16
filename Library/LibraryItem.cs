using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class LibraryItem
    {
        public LibraryItem(string name)
        {
            Name = name;
        }

        /// <summary>
        /// this represents the name for the item
        /// </summary>
        /// <value>name of the item</value>
        public string Name { get; set; }

        /// <summary>
        /// for checking if the item has been issued or not
        /// </summary>
        /// <value>issued status</value>
        public bool Booked
        {
            get => default(int);
            set
            {
            }
        }
    }
}
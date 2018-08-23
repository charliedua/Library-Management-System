using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Room : LibraryItem, IIssuable
    {
        private bool _acquired;

        public Room(string name, string identifier) : base(name, identifier)
        {
            _acquired = false;
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
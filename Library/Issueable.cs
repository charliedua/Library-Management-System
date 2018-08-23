using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class IssuableItem : LibraryItem
    {
        protected bool _acquired;

        public IssuableItem(string name, string identifier) : base(name, identifier)
        {
        }

        public abstract bool Give();

        public bool IsAvailable()
        {
            return !_acquired;
        }

        public abstract bool Take();
    }
}
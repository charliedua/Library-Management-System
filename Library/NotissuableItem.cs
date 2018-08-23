using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class NotissuableItem : LibraryItem
    {
        public NotissuableItem(string name, string identifier) : base(name, identifier)
        {
        }
    }
}
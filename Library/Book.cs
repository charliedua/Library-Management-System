using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Book : IssuableItem, IIssuable
    {
        public Book(string name, string identifier) : base(name, identifier)
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
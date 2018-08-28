using System;

namespace Library
{
    public class Room : IIssuable
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
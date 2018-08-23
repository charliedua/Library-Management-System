using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class Entity
    {
        protected string _identifier;
        protected string _name;

        public Entity(string name, string identifier)
        {
            _identifier = identifier;
            _name = name;
        }

        public string Identifier { get => _identifier; set => _identifier = value; }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Verifies the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool AreYou(string id) => id == _identifier;
    }
}
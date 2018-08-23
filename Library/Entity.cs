using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class Entity
    {
        protected string _identifier;

        public Entity(string identifier)
        {
            _identifier = identifier;
        }

        public Entity(string name, string identifier) : this(identifier)
        {
        }

        protected string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Identifier
        {
            get => _identifier;
            set => _identifier = value;
        }

        /// <summary>
        /// Verifies the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool Verify(string id) => id == _identifier;
    }
}
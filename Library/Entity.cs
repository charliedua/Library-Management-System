using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class Entity
    {
        protected string _identifier;

        public Entity(string name, string identifier)
        {
            _identifier = identifier;
        }

        protected string _name;

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
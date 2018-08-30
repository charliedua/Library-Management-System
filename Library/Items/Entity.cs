using System;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class from Database.
        /// </summary>
        /// <param name="ident">The identifier.</param>
        public Entity(string ident)
        {
            Load(ident);
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

        /// <summary>
        /// Loads this instance from db.
        /// </summary>
        public abstract void Load(string ident);

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        public abstract void Save();
    }
}
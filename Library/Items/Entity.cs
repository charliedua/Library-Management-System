using System;
using System.Collections.Generic;

namespace Library
{
    public abstract class Entity
    {
        protected string _identifier;
        protected string _name;

        /// <summary>
        /// The table name
        /// </summary>
        protected virtual string TABLE_NAME { get; }

        /// <summary>
        /// The col names in the table
        /// </summary>
        protected readonly List<string> COL_NAMES = new List<string>() { "Identifier", "Name" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="identifier">The identifier.</param>
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

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier
        {
            get => _identifier;
            set => _identifier = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
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
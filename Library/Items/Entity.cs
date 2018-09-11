using System.Collections.Generic;
using System.Data.SQLite;

namespace Library
{
    public abstract class Entity : ISavable
    {
        protected int _id;
        protected string _name;

        /// <summary>
        /// The table name
        /// </summary>
        public virtual string TABLE_NAME { get; }

        /// <summary>
        /// The col names in the table
        /// </summary>
        protected readonly List<string> COL_NAMES = new List<string>() { "ID", "Name" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Entity(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// with complete params.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ident">The ident.</param>
        public Entity(string name, int ident) : this(name)
        {
            _id = ident;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID
        {
            get => _id;
            set => _id = value;
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
        /// Gets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public virtual string Details
        {
            get
            {
                return string.Format("Name: {0} \nID: {1}\n", Name, ID);
            }
        }

        /// <summary>
        /// Verifies the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool AreYou(int id) => id == _id;

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        public abstract void Save();
    }
}
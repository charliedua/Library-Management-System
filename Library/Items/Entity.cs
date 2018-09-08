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
        protected virtual string TABLE_NAME { get; }

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
        /// Initializes a new instance of the <see cref="Entity"/> class from Database.
        /// </summary>
        /// <param name="ident">The identifier.</param>
        public Entity(int ident)
        {
            Database database = new Database();
            Load(database.LoadReader(TABLE_NAME, string.Format("ID = {0}", ident)));
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
        /// Verifies the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool AreYou(int id) => id == _id;

        #region Legacy Support

        public Entity Load(SQLiteDataReader ident)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        #endregion Legacy Support
    }
}
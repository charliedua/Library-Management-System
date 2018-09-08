using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

/// <summary>
/// The main namespace
/// </summary>
namespace Library
{
    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public class LibraryItem : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public LibraryItem(string name) : base(name)
        {
            Database database = new Database();
            _id = database.GetLastInsertedID(TABLE_NAME) + 1;
            database.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class from the database.
        /// </summary>
        /// <param name="ident">The identifier.</param>
        public LibraryItem(int ident) : base(ident)
        {
        }

        /// <summary>
        /// The table name
        /// </summary>
        protected override string TABLE_NAME => "Items";

        #region Database Stuff

        /// <summary>
        /// Loads this instance from db.
        /// </summary>
        /// <param name="ident"></param>
        public override void Load(int ident)
        {
            Database database = new Database();

            SQLiteDataReader reader = database.LoadReader("Items", string.Format("ID = '{0}'", ident));
            if (reader.HasRows)
            {
                reader.Read();
                _id = reader.GetInt32(0);
                _name = reader.GetString(1);
            }

            reader.Close();
            database.Disconnect();
        }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        public override void Save()
        {
            List<string> colVals = new List<string>() { _id.ToString(), _name };
            Database database = new Database();
            database.Save(TABLE_NAME, COL_NAMES, colVals);
        }

        #endregion Database Stuff
    }
}
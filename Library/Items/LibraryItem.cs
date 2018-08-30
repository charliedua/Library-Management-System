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
        public LibraryItem(string name, string identifier) : base(name, identifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class from the database.
        /// </summary>
        /// <param name="ident">The identifier.</param>
        public LibraryItem(string ident) : base(ident)
        {
        }

        /// <summary>
        /// Loads this instance from db.
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public override void Load(string ident)
        {
            Database database = new Database();

            SQLiteDataReader reader = (SQLiteDataReader)database.Load("Items", string.Format("Identifier = '{0}'", ident));
            if (reader.HasRows)
            {
                reader.Read();
                _identifier = reader.GetString(1);
                _name = reader.GetString(2);
            }

            reader.Close();
            database.Disconnect();
        }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        public override void Save()
        {
            string[] colVals = new string[] { _identifier, _name };
            string[] colnames = new string[] { "Identifier", "Name" };
            Database database = new Database();
            database.Save("Items", colnames, colVals);
        }
    }
}

/*
 Database<LibraryItem> database = new Database<LibraryItem>();
            database.Connect();
            database.Save("Items", new string[] { "" }, new string[] { });

     */
/// <summary>
/// The main namespace
/// </summary>
namespace Library
{
    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public abstract class LibraryItem : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public LibraryItem(string name, string identifier) : base(name, identifier)
        {
        }
    }
}

/*
 Database<LibraryItem> database = new Database<LibraryItem>();
            database.Connect();
            database.Save("Items", new string[] { "" }, new string[] { });

     */
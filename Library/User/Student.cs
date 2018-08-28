namespace Library
{
    public class Student : User, ICanIssue
    {
        private Inventory _inventory;

        public Student(string name, string identifier) : base(name, identifier)
        {
        }

        public Inventory Inventory { get; set; }
        public int NumberOfItemsAcquired { get => _inventory.NumberOfItems; }

        public bool Acquire(LibraryItem item)
        {
            _inventory.Put(item);
            return true;
        }

        public bool Release(LibraryItem item)
        {
            _inventory.Take(item);
            return _inventory.Has(item);
        }
    }
}
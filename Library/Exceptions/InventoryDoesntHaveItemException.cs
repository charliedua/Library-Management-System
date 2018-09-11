using System;
using System.Runtime.Serialization;

namespace Library
{
    [Serializable]
    internal class InventoryDoesntHaveItemException : Exception
    {
        public InventoryDoesntHaveItemException(LibraryItem item, User user) : base(string.Format("The Item with ID: [{0}] could not be located in the Inventory of the user with ID: [{1}].", item.ID, user.ID))
        {
        }

        public InventoryDoesntHaveItemException(string message) : base(message)
        {
        }

        public InventoryDoesntHaveItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InventoryDoesntHaveItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
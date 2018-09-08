using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Tests
{
    [TestClass()]
    public class InventoryTests
    {
        [TestMethod()]
        public void NotHasTest()
        {
            Inventory inventory = new Inventory();
            Assert.IsFalse(inventory.Has(12));
        }
    }
}
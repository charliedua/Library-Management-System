using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    [TestClass()]
    public class InventoryTests
    {
        [TestMethod()]
        public void NotHasTest()
        {
            Inventory inventory = new Inventory();
            Assert.IsFalse(inventory.Has("bizare"));
        }
    }
}
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
    public class LibraryItemTests
    {
        [TestMethod()]
        public void SaveLoadTest()
        {
            LibraryItem item = new LibraryItem("cat");
            item.Save();
            item = new LibraryItem(item.ID);
            Assert.AreEqual(item.Name, "cat");
        }
    }
}
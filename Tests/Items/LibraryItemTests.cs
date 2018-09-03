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
            LibraryItem item = new LibraryItem("cat", "1457");
            item.Save();
            item = null;
            item = new LibraryItem("1457");
            Assert.AreEqual(item.Name, "cat");
        }
    }
}
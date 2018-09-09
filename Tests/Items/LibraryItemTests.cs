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
            Database database = new Database();
            item = LibraryItem.Load(database.LoadReader(item.TABLE_NAME, string.Format("ID = {0}", item.ID)));
            Assert.AreEqual(item.Name, "cat");
        }
    }
}
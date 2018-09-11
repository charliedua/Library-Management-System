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
    public class EntityTests
    {
        [TestMethod()]
        public void SaveTest()
        {
            Entity entity = new LibraryItem("charlie");
            entity.Save();
            entity = new User("charlie");
            entity.Save();
        }
    }
}
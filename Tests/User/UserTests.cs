using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// NOTE: Make sure the architectures mach (x64)
namespace Library.Tests
{
    [TestClass()]
    public class UserTests
    {
        [TestMethod()]
        public void SaveLoadTest()
        {
            User user = new User("cat", "145");
            user.Save();
            user = null;
            user = new User("145");
            Assert.AreEqual(user.Name, "cat");
        }
    }
}
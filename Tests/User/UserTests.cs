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
            User user = new User("cat");
            user.Save();
            user = new User(user.ID);
            Assert.AreEqual(user.Name, "cat");
        }
    }
}
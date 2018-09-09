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
            Database database = new Database();
            user = User.Load(database.LoadReader(user.TABLE_NAME, string.Format("ID = {0}", user.ID)));
            Assert.AreEqual(user.Name, "cat");
        }

        [TestMethod()]
        public void LoginTest()
        {
            User user = new User("charlie");
            user.CreateAccount("charlie", "password");
            Assert.IsTrue(user.Login("charlie", "password"));
        }

        [TestMethod()]
        public void CreateAccountTest()
        {
            User user = new User("charlie");
            user.CreateAccount("charlie", "password");
            Assert.AreEqual("charlie", user.Account.Username);
        }

        [TestMethod()]
        public void LogoutTest()
        {
            User user = new User("charlie");
            user.CreateAccount("charlie", "password");
            user.Login("charlie", "password");
            Assert.IsTrue(user.IsAuthenticated);
            Assert.IsTrue(user.Logout());
            Assert.IsFalse(user.IsAuthenticated);
        }
    }
}
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
            Assert.IsTrue(user.state == UserState.LoggedOut);
        }

        [TestMethod()]
        public void IssueTest()
        {
            User user = GetAuthenticatedUser();
            LibraryItem item = new LibraryItem("UCD");
            user.Issue(item);
            Assert.IsTrue(user.HasItem(item.ID));
            user = null;
        }

        [TestMethod()]
        public void ReturnTest()
        {
            LibraryItem item = new LibraryItem("UCD");
            var user = GetAuthenticatedUser();
            user.Issue(item);
            user.Return(item);
            Assert.IsFalse(user.HasItem(item.ID));
            user = null;
        }

        private static User GetAuthenticatedUser()
        {
            User user = new User("charlie", 5, new UserAccount("A", "B"), UserState.LoggedIN, new List<Permissions>() { Permissions.None });
            user.Login("A", "P");
            return user;
        }
    }
}
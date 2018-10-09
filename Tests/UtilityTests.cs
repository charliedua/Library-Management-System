using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utils.Tests
{
    [TestClass()]
    public class UtilityTests
    {
        [TestMethod()]
        public void LoadAllItemsTest()
        {
            List<LibraryItem> items = Utility.LoadAllItems();
        }

        [TestMethod()]
        public void LoadAllUsersTest()
        {
            List<User> users = Utility.LoadAllUsers();
        }

        [TestMethod()]
        public void LoadInventoryTest()
        {
            List<User> users = Utility.LoadAllUsers();
            List<LibraryItem> items = Utility.LoadAllItems();
            users.ForEach(x => { if (x.HasAccount) x.Account.State = UserState.LoggedIN; });
            Utility.LoadInventory(users, items);
        }

        [TestMethod()]
        public void SaveAllItemsTest()
        {
            List<LibraryItem> items = new List<LibraryItem>();
            items.Add(new LibraryItem(1, "cat"));
            Utility.SaveAllItems(items);
        }

        [TestMethod()]
        public void SaveAllUsersTest()
        {
            List<User> users = new List<User>();
            users.Add(new User("fanboi", 1));
            users[0].MaxItems = 10;
            users[0].GivePermission(Permissions.Create);
            users[0].GivePermission(Permissions.Delete);
            users[0].CreateAccount("fanboi", "cat");
            Utility.SaveAllUsers(users);
        }
    }
}
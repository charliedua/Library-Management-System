using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Contains utility methods
    /// </summary>
    internal static class Utility
    {
        public static bool Login(User user, string password)
        {
            return user.Authenticate(password);
        }

        public static bool Login(string _identifier, string password)
        {
            List<User> users = Database<List<User>>.Load<List<User>>("users.json");
            var temp = users.Find(x => x.AreYou(_identifier));
            return temp != null ? temp.Authenticate(password) : false;
        }

        public static bool Login(List<User> users, string _identifier, string password)
        {
            var temp = users.Find(x => x.AreYou(_identifier));
            return temp != null ? temp.Authenticate(password) : false;
        }
    }
}
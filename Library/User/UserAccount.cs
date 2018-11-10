using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Library
{
    /// <summary>
    /// This is the account for the user.
    /// </summary>
    public class UserAccount
    {
        private string _password;
        private string _username;
        public User user;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccount" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="user">The user.</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        public UserAccount(string username, string password, User user, bool encrypt = true)
        {
            _username = username;
            _password = encrypt ? GetSha256Hash(password) : password;
            State = UserState.LoggedOut;
            this.user = user;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = GetSha256Hash(value);
            }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public UserState State { get; set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public bool IsAuthenticated { get => State == UserState.LoggedIN; }

        public void SetPassword(string password)
        {
            _password = GetSha256Hash(password);
        }

        /// <summary>
        /// Verifies the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="pass">The pass.</param>
        /// <returns></returns>
        public bool VerifyPassword(string username, string pass)
        {
            return username == _username && VerifySha256Hash(pass, _password);
        }

        /// <summary>
        /// Authenticates user with the specified username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>status</returns>
        /// <exception cref="DoesNotHaveAccountException"></exception>
        public bool Login(string username, string password)
        {
            if (!user.HasAccount)
            {
                throw new DoesNotHaveAccountException();
            }
            State = UserState.LoggedIN;
            return VerifyPassword(username: username, pass: password);
        }

        /// <summary>
        /// Logouts this user.
        /// </summary>
        /// <returns>success status</returns>
        public bool Logout()
        {
            if (State == UserState.LoggedIN)
            {
                State = UserState.LoggedOut;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Encryption Stuff

        /// <summary>
        /// Gets the sha256 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string GetSha256Hash(string input)
        {
            using (SHA256 Sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = Sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Verifies the sha256 hash against a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>the validity</returns>
        private static bool VerifySha256Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetSha256Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        #endregion Encryption Stuff
    }
}
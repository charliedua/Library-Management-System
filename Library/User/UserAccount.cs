using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Library
{
    public class UserAccount
    {
        private readonly string _password;
        private readonly string _username;

        public UserAccount(string username, string password)
        {
            _username = username;
            _password = GetSha256Hash(password);
        }

        public UserAccount() : this("", "")
        {
        }

        public string Password => _password;

        public string Username => _username;

        public UserState State { get; private set; }

        public bool VerifyPassword(string username, string pass)
        {
            return VerifySha256Hash(pass, _password);
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
        /// <returns></returns>
        private static bool VerifySha256Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetSha256Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        #endregion Encryption Stuff

        public void Save(List<string> COL_NAMES, List<string> colvalues)
        {
            COL_NAMES.AddRange(new string[] { "Username", "Password" });
            colvalues.AddRange(new string[] { Username, Password });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class User : Entity
    {
        private bool _isAuthenticated;
        private string _passPhrase;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name, string identifier) : base(name, identifier)
        {
            _isAuthenticated = false;
            _passPhrase = null;
        }

        public bool Authenticate()
        {
            throw new NotImplementedException();
        }
    }
}
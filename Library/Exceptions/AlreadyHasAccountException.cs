using System;
using System.Runtime.Serialization;

namespace Library
{
    [Serializable]
    internal class AlreadyHasAccountException : Exception
    {
        public AlreadyHasAccountException(User user) : base(string.Format("The user {0} already has an account, ID: {1}", user.Name, user.ID))
        {
        }

        public AlreadyHasAccountException(string message) : base(message)
        {
        }

        public AlreadyHasAccountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyHasAccountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
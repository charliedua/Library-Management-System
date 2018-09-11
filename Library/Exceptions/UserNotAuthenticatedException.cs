using System;
using System.Runtime.Serialization;

namespace Library
{
    [Serializable]
    internal class UserNotAuthenticatedException : Exception
    {
        public UserNotAuthenticatedException(User user) : base(string.Format("The user with ID: {0} is not authenticated", user.ID))
        {
        }

        public UserNotAuthenticatedException(string message) : base(message)
        {
        }

        public UserNotAuthenticatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotAuthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
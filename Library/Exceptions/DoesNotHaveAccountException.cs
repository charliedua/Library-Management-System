using System;

namespace Library
{
    [Serializable]
    public class DoesNotHaveAccountException : Exception
    {
        public DoesNotHaveAccountException() : base("User doesn't have a account")
        {
        }

        public DoesNotHaveAccountException(string message) : base(message)
        {
        }

        public DoesNotHaveAccountException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DoesNotHaveAccountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
using System;

namespace Library
{
    [Serializable]
    public class DoesNotHavePermissionException : Exception
    {
        public DoesNotHavePermissionException(Permissions permissions) : this(string.Format("You don't have permission to {0} this instance.", permissions.ToString()))
        {
        }

        public DoesNotHavePermissionException(string message) : base(message)
        {
        }

        public DoesNotHavePermissionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DoesNotHavePermissionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
using System;
using System.Runtime.Serialization;

namespace Library.Commands
{
    [Serializable]
    internal class InvalidCommandSyntaxException : Exception
    {
        public InvalidCommandSyntaxException(string expected) : base($"Invalid Command Syntax, Expected: {expected}")
        {
        }

        public InvalidCommandSyntaxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
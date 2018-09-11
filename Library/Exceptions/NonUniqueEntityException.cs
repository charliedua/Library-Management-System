using System;
using System.Runtime.Serialization;

namespace Library
{
    [Serializable]
    internal class NonUniqueEntityException : Exception
    {
        public NonUniqueEntityException(string name, string value) : base(string.Format("The UNIQUE constraint failed on {0}: value: {1}", name, value))
        {
        }

        public NonUniqueEntityException(string message) : base(message)
        {
        }

        public NonUniqueEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonUniqueEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
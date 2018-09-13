using System;
using System.Runtime.Serialization;

namespace Library.Commands
{
    [Serializable]
    internal class InvalidCommandParamatersException : Exception
    {
        public InvalidCommandParamatersException(string got) :
            base(string.Format("The params passed to the command are invalid\n\tGot: {0}", got))
        {
        }

        public InvalidCommandParamatersException(Type Expected, Type Given) :
            base(string.Format("The params passed to the command are invalid \n Expected: {0}\n Given: {1}", Expected.ToString(), Given.ToString()))
        {
        }

        public InvalidCommandParamatersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandParamatersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
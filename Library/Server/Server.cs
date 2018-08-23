using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public abstract class Server
    {
        /// <summary>
        /// Connects to server.
        /// </summary>
        /// <returns>status of <c>connection</c></returns>
        public abstract bool Connect();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public interface IIssuable
    {
        /// <summary>
        /// To return or release the resource captured
        /// </summary>
        /// <returns>
        /// the status is it was possible or not
        /// </returns>
        bool Give();

        /// <summary>
        /// Determines whether this instance is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </returns>
        bool IsAvailable();

        /// <summary>
        /// To or acquire the resource required.
        /// </summary>
        /// <returns>
        /// the status is it was possible or not.
        /// </returns>
        bool Take();
    }
}
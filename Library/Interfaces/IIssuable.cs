namespace Library
{
    public interface IIssuable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IIssuable"/> is acquired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if acquired; otherwise, <c>false</c>.
        /// </value>
        bool Acquired { get; set; }

        /// <summary>
        /// Gives this instance.
        /// </summary>
        /// <returns></returns>
        bool Give();

        /// <summary>
        /// Takes this instance.
        /// </summary>
        /// <returns></returns>
        bool Take();
    }
}
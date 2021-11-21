namespace bebzs.DateTimeRange
{
    /// <summary>
    /// Defines the interval types.
    /// </summary>
    public enum Interval
    {
        /// <summary>
        /// The 2 parts are included.
        /// </summary>
        Close,
        /// <summary>
        /// The 2 parts are excluded.
        /// </summary>
        Open,
        /// <summary>
        /// The left part is excluded, the right part is included.
        /// </summary>
        LeftOpenRightClose,
        /// <summary>
        /// The left part is included, the right part is excluded.
        /// </summary>
        LeftCloseRightOpen
    }
}

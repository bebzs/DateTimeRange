namespace bebzs.DateTimeRange
{
    /// <summary>
    /// Defines the instersection type.
    /// </summary>
    public enum Intersection
    {
        /// <summary>
        /// No intersection.
        /// </summary>
        None,
        /// <summary>
        /// The given range is partially conatined in the range.
        /// </summary>
        PartiallyInRange,
        /// <summary>
        /// The given ranges are equals.
        /// </summary>
        RangesEqualed,
        /// <summary>
        /// The given range is contained in the range.
        /// </summary>
        ContainedInRange,
        /// <summary>
        /// The given range contains the range.
        /// </summary>
        ContainsRange
    }
}

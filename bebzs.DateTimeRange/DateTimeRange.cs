using System;

namespace bebzs.DateTimeRange
{
    /// <summary>
    /// Defines a DateTime range object.
    /// </summary>
    public struct DateTimeRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/>.
        /// </summary>
        /// <param name="start">The start of the <see cref="DateTimeRange"/>.</param>
        /// <param name="end">The end of the <see cref="DateTimeRange"/>.</param>
        public DateTimeRange(DateTime start, DateTime end)
            : this(start, end, Interval.Close)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/>.
        /// </summary>
        /// <param name="start">The start of the <see cref="DateTimeRange"/>.</param>
        /// <param name="end">The end of the <see cref="DateTimeRange"/>.</param>
        /// <param name="interval">The <see cref="IntervalType"/>.</param>
        public DateTimeRange(DateTime start, DateTime? end, Interval interval = Interval.LeftCloseRightOpen)
        {
            if (!end.HasValue && (interval != Interval.Open || interval != Interval.LeftCloseRightOpen))
            {
                throw new ArgumentException("The interval type is incorrect because the end has no value.", "interval");
            }

            if (end.HasValue && start >= end.Value)
            {
                throw new ArgumentException("Start is bigger than End.", "start");
            }

            Start = start;
            End = end;
            Interval = interval;
        }

        /// <summary>
        /// The start of the <see cref="DateTimeRange"/>.
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// The end of the <see cref="DateTimeRange"/>.
        /// </summary>
        public DateTime? End { get; private set; }

        /// <summary>
        /// The <see cref="IntervalType"/> of the <see cref="DateTimeRange"/>.
        /// </summary>
        public Interval Interval { get; set; }

        #region "Operators"

        /// <summary>
        /// Determines whether two specified instances of System.DateTime are equal.
        /// </summary>
        /// <param name="rangeLeft">The first object to compare.</param>
        /// <param name="rangeRight">The second object to compare.</param>
        /// <returns>true if d1 and d2 represent the same date and time range; otherwise, false.</returns>
        public static bool operator ==(DateTimeRange rangeLeft, DateTimeRange rangeRight)
        {
            return rangeLeft.Equals(rangeRight);
        }

        /// <summary>
        /// Determines whether two specified instances of System.DateTime are not equal.
        /// </summary>
        /// <param name="rangeLeft">The first object to compare.</param>
        /// <param name="rangeRight">The second object to compare.</param>
        /// <returns>true if d1 and d2 do not represent the same date and time range; otherwise, false.</returns>
        public static bool operator !=(DateTimeRange rangeLeft, DateTimeRange rangeRight)
        {
            return !(rangeLeft == rangeRight);
        }

        #endregion

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DateTimeRange dtr)
            {
                return this.Start == dtr.Start && this.End == dtr.End && this.Interval == dtr.Interval;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Get the <see cref="DateTimeRange"/> which results of the instersection of the given <see cref="DateTimeRange"/> with the current instance.
        /// </summary>
        /// <param name="range">The <see cref="DateTimeRange"/> to compare width the current instance.</param>
        /// <returns>A <see cref="DateTimeRange"/> which results of the instersection of the given <see cref="DateTimeRange"/> with the current instance.</returns>
        public DateTimeRange GetIntersection(DateTimeRange range)
        {
            Intersection type = this.GetIntersectionType(range);
            if (type == Intersection.RangesEqualed || type == Intersection.ContainedInRange)
            {
                return range;
            }
            else if (type == Intersection.PartiallyInRange)
            {
                if (IsInRange(range.Start))
                {
                    return new DateTimeRange(range.Start, this.End);
                }
                else
                {
                    return new DateTimeRange(this.Start, range.End);
                }
            }
            else if (type == Intersection.ContainsRange)
            {
                return this;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Determines the <see cref="Intersection"/> of the given <see cref="DateTimeRange"/> with the current instance.
        /// </summary>
        /// <param name="range">The <see cref="DateTimeRange"/> to compare with the current instance.</param>
        /// <returns>An <see cref="Intersection"/>.</returns>
        public Intersection GetIntersectionType(DateTimeRange range)
        {
            if (this == range)
            {
                return Intersection.RangesEqualed;
            }
            else if (IsInRange(range.Start) && (!range.End.HasValue || (range.End.HasValue && IsInRange(range.End.Value))))
            {
                return Intersection.ContainedInRange;
            }
            else if (IsInRange(range.Start))
            {
                return Intersection.PartiallyInRange;
            }
            else if (IsInRange(range.End.Value))
            {
                return Intersection.PartiallyInRange;
            }
            else if (range.IsInRange(this.Start) && (!this.End.HasValue || (this.End.HasValue && range.IsInRange(this.End.Value))))
            {
                return Intersection.ContainsRange;
            }

            return Intersection.None;
        }

        /// <summary>
        /// Determines if the given <see cref="DateTimeRange"/> intersects this instance.
        /// </summary>
        /// <param name="range">The <see cref="DateTimeRange"/> to compare with the current instance.</param>
        /// <returns>true if the <paramref name="range"/> intersects the current instance; otherwise, false.</returns>
        public bool Intersects(DateTimeRange range)
        {
            return GetIntersectionType(range) != Intersection.None;
        }

        /// <summary>
        /// Determines if the given <see cref="DateTime"/> is in the current <see cref="DateTimeRange"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> to compare with the current <see cref="DateTimeRange"/>.</param>
        /// <returns>true if the <paramref name="date"/> is in the current <see cref="DateTimeRange"/>; otherwise, false.</returns>
        public bool IsInRange(DateTime date)
        {
            switch (this.Interval)
            {
                case Interval.Open:
                    return date > this.Start && date < this.End;
                case Interval.LeftCloseRightOpen:
                    return date >= this.Start && date < this.End;
                case Interval.LeftOpenRightClose:
                    return date > this.Start && date <= this.End;
                default:
                    return date >= this.Start && date <= this.End;
            }
        }

        /// <summary>
        /// Converts the value of the current System.DateTimeRange object to its equivalent string representation using the formatting conventions of the current culture.
        /// </summary>
        /// <returns>A string representation of the value of the current System.DateTime object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The date and time are outside the range of dates supported by the calendar used by the current culture.</exception>
        public override string ToString()
        {
            string result = string.Empty;
            if (this.End.HasValue)
            {
                result = this.Start.ToString() + ", " + this.End.ToString();
            }
            else
            {
                result = this.Start.ToString() + ", ";
            }

            switch (Interval)
            {
                case Interval.Open:
                    result = "]" + result + "[";
                    break;
                case Interval.LeftCloseRightOpen:
                    result = "[" + result + "[";
                    break;
                case Interval.LeftOpenRightClose:
                    result = "]" + result + "]";
                    break;
                default:
                    result = "[" + result + "]";
                    break;
            }

            return result;
        }
    }
}

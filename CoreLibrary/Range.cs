using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreLibrary
{
    public static class Range
    {
        public enum BoundTypes : byte
        {
            Exclusive,
            Inclusive,
            Infinite
        }

        public static Range<T> Parse<T>(string strValue) where T : IComparable<T>
        {
            if (string.IsNullOrEmpty(strValue))
                throw new ArgumentNullException(nameof(strValue));

            var str = strValue.Trim();
            if (str.Length < 3)
                throw new FormatException("Malformed range literal.");

            if (string.Equals(str, "empty", StringComparison.OrdinalIgnoreCase))
                return Range<T>.Empty;

            var pattern = new Regex(@"^([\[\(]{1})(\S*),(\S*)([\]\)]{1})$");
            var match = pattern.Match(str);

            if (!match.Success)
            {
                throw new FormatException("Invalid range format");
            }

            if (match.Groups.Count != 5)
                throw new FormatException("Invalid range format");

            var lowerBoundType = BoundTypes.Infinite;
            var upperBoundType = BoundTypes.Infinite;
            var lowerBound = default(T);
            var upperBound = default(T);

            var boundConverter = TypeDescriptor.GetConverter(typeof(T));

            if (match.Groups[2].Length > 0)
            {
                lowerBoundType = (match.Groups[1].Value == "[") ? BoundTypes.Inclusive : BoundTypes.Exclusive;
                lowerBound = (T)boundConverter.ConvertFromString(match.Groups[2].Value);
            }

            if (match.Groups[3].Length > 0)
            {
                upperBoundType = (match.Groups[4].Value == "]") ? BoundTypes.Inclusive : BoundTypes.Exclusive;
                upperBound = (T)boundConverter.ConvertFromString(match.Groups[3].Value);
            }

            return new Range<T>(lowerBound, upperBound, lowerBoundType, upperBoundType);
        }
    }

    public struct Range<T> where T : IComparable<T>
    {
        public static Range<T> Empty =
            new Range<T>(default(T), default(T), Range.BoundTypes.Exclusive, Range.BoundTypes.Exclusive);

        public override bool Equals(object o)
        {
            object obj;
            if ((obj = o) is Range<T>)
                return this.Equals((Range<T>)obj);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(LowerBound);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(UpperBound);
                hashCode = (hashCode * 397) ^ ((byte)LowerBoundType >> 8 + (byte)UpperBoundType);
                return hashCode;
            }
        }

        public bool Equals(Range<T> range)
        {
            if (LowerBoundType != range.LowerBoundType || UpperBoundType != range.UpperBoundType)
                return false;

            return LowerBound.CompareTo(range.LowerBound) == 0 && UpperBound.CompareTo(range.UpperBound) == 0;
        }

        public T LowerBound { get; set; }
        public T UpperBound { get; set; }

        public Range.BoundTypes LowerBoundType { get; set; }
        public Range.BoundTypes UpperBoundType { get; set; }

        public static bool operator ==(Range<T> x, Range<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Range<T> x, Range<T> y)
        {
            return !x.Equals(y);
        }

        public Range(T lowerBound, T upperBound, Range.BoundTypes lowerBoundType = Range.BoundTypes.Inclusive,
            Range.BoundTypes upperBoundType = Range.BoundTypes.Inclusive)
        {
            if (lowerBoundType != Range.BoundTypes.Infinite && upperBoundType != Range.BoundTypes.Infinite)
            {
                var compare = lowerBound.CompareTo(upperBound);
                if (compare > 0)
                    throw new FormatException("Invalid range. Lower bound must be less or equal to upper bound!");
            }

            LowerBound = lowerBound;
            UpperBound = upperBound;

            LowerBoundType = lowerBoundType;
            UpperBoundType = upperBoundType;
        }

        public bool Contains(T obj)
        {
            if (LowerBoundType == Range.BoundTypes.Infinite && UpperBoundType == Range.BoundTypes.Infinite)
                return true;

            var lowerCompare = obj.CompareTo(LowerBound);
            var upperCompare = obj.CompareTo(UpperBound);

            var moreThanUpper = upperCompare > 0 || (upperCompare == 0 && UpperBoundType != Range.BoundTypes.Inclusive);
            var lessThanLower = lowerCompare < 0 || (lowerCompare == 0 && LowerBoundType != Range.BoundTypes.Inclusive);

            if (LowerBoundType == Range.BoundTypes.Infinite && !moreThanUpper)
                return true;
            if (UpperBoundType == Range.BoundTypes.Infinite && !lessThanLower)
                return true;

            return !moreThanUpper && !lessThanLower;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(LowerBoundType == Range.BoundTypes.Inclusive ? '[' : '(');
            if (LowerBoundType != Range.BoundTypes.Infinite)
                stringBuilder.Append(LowerBound.ToString());
            stringBuilder.Append(',');
            if (UpperBoundType != Range.BoundTypes.Infinite)
                stringBuilder.Append(UpperBound.ToString());
            stringBuilder.Append(UpperBoundType == Range.BoundTypes.Inclusive ? ']' : ')');
            return stringBuilder.ToString();
        }
    }
}
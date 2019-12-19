using System;
using CoreLibrary;

namespace DbParametersBuilder.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class RangeAttribute : Attribute {
        public RangeAttribute(string lowerBound, string upperBound, Range.BoundTypes lowerBoundType = Range.BoundTypes.Exclusive, Range.BoundTypes upperBoundType = Range.BoundTypes.Exclusive) {
            LowerBound = lowerBound;
            UpperBound = upperBound;

            LowerBoundType = lowerBoundType;
            UpperBoundType = upperBoundType;
        }
        public string LowerBound { get; }
        public string UpperBound { get; }
        public Range.BoundTypes LowerBoundType { get; }
        public Range.BoundTypes UpperBoundType { get; }
    }
}
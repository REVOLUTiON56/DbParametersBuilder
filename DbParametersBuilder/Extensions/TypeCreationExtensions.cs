using System;
using System.Collections.Generic;
using CoreLibrary;
using NpgsqlTypes;

namespace SqlCore.DLBase.FilterBuilder.Extensions {
    public static class TypesCreationExtensions {
        public static NpgsqlRange<T> ConvertToNpgsqlRange<T>(this Range<T> range) where T : IComparable<T> {
            return new NpgsqlRange<T>(range.LowerBound, range.LowerBoundType == Range.BoundTypes.Inclusive, range.LowerBoundType == Range.BoundTypes.Infinite,
                range.UpperBound, range.UpperBoundType == Range.BoundTypes.Inclusive, range.UpperBoundType == Range.BoundTypes.Infinite);
        }

        public static IList<T> CreateList<T>() {
            return new List<T>();
        }

        public static T[] CreateArray<T>(int length) {
            return new T[length];
        }

        public static NpgsqlRange<T> CreateNpgsqlRange<T>(T lowerBound, bool isLowerBoundInclusive, bool isLowerBoundInfinite, T upperBound, bool isUpperBoundInclusive, bool isUpperBoundInfinite) {
            return new NpgsqlRange<T>(lowerBound, isLowerBoundInclusive, isLowerBoundInfinite, upperBound, isUpperBoundInclusive, isUpperBoundInfinite);
        }

        public static Range<T> CreateRange<T>(T lowerBound, T upperBound, Range.BoundTypes lowerBoundType, Range.BoundTypes upperBoundType) where T : IComparable<T> {
            return new Range<T>(lowerBound, upperBound, lowerBoundType, upperBoundType);
        }
    }
}
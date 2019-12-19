using System;
using System.Reflection;
using CoreLibrary;
using DbParametersBuilder.Attributes;

namespace DbParametersBuilder.Data.Shared {
    /// <summary>
    /// Разделяемые данные между всеми параметрами. Они одни для всех параметров
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbParameterSharedRangeData<T> : DbParameterSharedData<T> {
        public string LowerBoundFilter { get; protected set; }
        public Range.BoundTypes LowerBoundType { get; protected set; }
        public string UpperBoundFilter { get; protected set; }
        public Range.BoundTypes UpperBoundType { get; protected set; }
        public Type ElementType => typeof(T).GetGenericArguments()[0];

        protected DbParameterSharedRangeData(PropertyInfo property) : base(property) {
        }

        public DbParameterSharedRangeData(PropertyInfo property, object[] typeAttributes) : this(property) {
            GetDataFromAttributes(property, typeAttributes);
            var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

            LowerBoundFilter = rangeAttribute?.LowerBound ?? string.Empty;
            UpperBoundFilter = rangeAttribute?.UpperBound ?? string.Empty;
            LowerBoundType = rangeAttribute?.LowerBoundType ?? Range.BoundTypes.Inclusive;
            UpperBoundType = rangeAttribute?.UpperBoundType ?? Range.BoundTypes.Inclusive;
        }
    }
}

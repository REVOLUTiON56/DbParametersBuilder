using System;
using CoreLibrary;
using DbParametersBuilder.Extensions;

namespace DbParametersBuilder.Data.Converters {

    public class RangeConverter<T> : IConverter<Range<T>> where T : IComparable<T> {
        public object Convert(Range<T> value) {
            var method = typeof(TypesCreationExtensions).GetMethod("ConvertToNpgsqlRange")?.MakeGenericMethod(typeof(T));
            if (method == null)
                throw new InvalidOperationException("Can't find ConvertToNpgsqlRange method");

            return method.Invoke(null, new object[] { value });
        }
    }
}
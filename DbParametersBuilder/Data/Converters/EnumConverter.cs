using System;
using DbParametersBuilder.Core;

namespace DbParametersBuilder.Data.Converters {
    public class EnumConverter<T> : IConverter<T> {
        public object Convert(T value) {
            var isMapped = NpgsqlTypeMapperCache.IsMapped(typeof(T));
            return isMapped
                ? value
                : System.Convert.ChangeType(value, typeof(T).GetEnumUnderlyingType());
        }
    }
}

using System;
using System.Collections;
using CoreLibrary;

namespace DbParametersBuilder.Data.Converters {
    public static class ConvertersFactory {
        public static IConverter<T> GetConverter<T>(Type type) {
            if(type.IsEnum)
                return new EnumConverter<T>();

            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string)) {
                if (type.IsArray)
                    return Activator.CreateInstance(typeof(EnumerableConverter<>).MakeGenericType(type.GetElementType())) as IConverter<T>;
                
                return Activator.CreateInstance(typeof(EnumerableConverter<>).MakeGenericType(type.GetGenericArguments()[0])) as IConverter<T>;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Range<>)) {
                var elementType = type.GetGenericArguments()[0];
                return Activator.CreateInstance(typeof(RangeConverter<>).MakeGenericType(elementType)) as IConverter<T>;
            }

            return new BaseConverter<T>();
        }
    }
}
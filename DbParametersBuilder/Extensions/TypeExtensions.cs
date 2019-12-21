using System;

namespace DbParametersBuilder.Extensions {
    public static class TypeExtensions {
        public static object GetDefaultValue(this Type type) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type != typeof(void) && type.IsValueType && Nullable.GetUnderlyingType(type) == null) {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static bool IsNullableType(this Type type) {
            if (!type.IsValueType)
                return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null)
                return true; // Nullable<T>
            return false; // value-type
        }
    }
}
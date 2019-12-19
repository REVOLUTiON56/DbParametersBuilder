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
    }
}
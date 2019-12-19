using System;
using NpgsqlTypes;

namespace DbParametersBuilder.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DbParameterTypeAttribute : Attribute {
        public DbParameterTypeAttribute(NpgsqlDbType type) {
            Type = type;
        }

        public NpgsqlDbType Type { get; } = NpgsqlDbType.Unknown;
    }
}
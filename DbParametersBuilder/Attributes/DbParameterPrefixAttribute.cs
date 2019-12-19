using System;

namespace DbParametersBuilder.Attributes {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class NpgSqlDbParametersPrefixAttribute : Attribute {
        public NpgSqlDbParametersPrefixAttribute(string prefix) {
            Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }
        public string Prefix { get; }
    }
}
using System;

namespace DbParametersBuilder.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DbParameterNameAttribute : Attribute {
        public DbParameterNameAttribute(string name) {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }
    }
}
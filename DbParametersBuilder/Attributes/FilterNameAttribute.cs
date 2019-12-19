using System;

namespace DbParametersBuilder.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FilterNameAttribute : Attribute {
        public FilterNameAttribute(string filterName) {
            if (string.IsNullOrEmpty(filterName))
                throw new ArgumentNullException(nameof(filterName));

            FilterName = filterName;
        }

        public string FilterName { get; }
    }
}
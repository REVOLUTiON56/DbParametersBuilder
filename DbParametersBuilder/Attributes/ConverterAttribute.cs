using System;
using DbParametersBuilder.Data.Converters;

namespace DbParametersBuilder.Attributes {
    public class ConverterAttribute : Attribute {
        public Type Type { get; }
        public ConverterAttribute(Type type) {
            if (!typeof(IConverter).IsAssignableFrom(type))
                throw new InvalidOperationException();
            Type = type;
        }
    }
}

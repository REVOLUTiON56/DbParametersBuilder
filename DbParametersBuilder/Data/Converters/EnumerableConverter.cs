using System.Collections.Generic;
using System.Linq;

namespace DbParametersBuilder.Data.Converters {
    public class EnumerableConverter<T> : IConverter<IEnumerable<T>> {
        public static IConverter<T> NestedConverter;
        public object Convert(IEnumerable<T> value) {
            return value.Select(x => NestedConverter.Convert(x)).ToArray();
        }

        static EnumerableConverter() {
            NestedConverter = ConvertersFactory.GetConverter<T>(typeof(T));
        }
    }
}

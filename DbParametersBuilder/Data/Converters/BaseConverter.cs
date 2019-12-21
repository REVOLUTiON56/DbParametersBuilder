namespace DbParametersBuilder.Data.Converters {
    public class BaseConverter<T> : IConverter<T> {
        public object Convert(T value) {
            return value;
        }
    }
}

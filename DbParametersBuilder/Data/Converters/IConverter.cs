namespace DbParametersBuilder.Data.Converters {
    public interface IConverter<in T> : IConverter {
        object Convert(T value);
    }

    public interface IConverter {
    }
}

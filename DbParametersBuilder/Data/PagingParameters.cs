using System.ComponentModel;
using DbParametersBuilder.Attributes;
using NpgsqlTypes;

namespace DbParametersBuilder.Data {
    public class PagingParameters {
        [FilterName("Limit")]
        [DbParameterType(NpgsqlDbType.Integer)]
        [DbParameterName("page_limit")]
        [DefaultValue(10)]
        public int Limit { get; }

        [FilterName("Start")]
        [DbParameterName("page_offset")]
        [DbParameterType(NpgsqlDbType.Integer)]
        [DefaultValue(0)]
        public int Offset { get; }
    }
}
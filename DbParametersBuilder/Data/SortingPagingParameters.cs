using System.ComponentModel;
using DbParametersBuilder.Attributes;
using NpgsqlTypes;

namespace DbParametersBuilder.Data {
    public class SortingPagingParameters : PagingParameters {
        [FilterName("SortColumn")]
        [DbParameterName("sort_col")]
        [DbParameterType(NpgsqlDbType.Text)]
        public string SortColumn { get; }

        [FilterName("SortDir")]
        [DbParameterName("sort_dir")]
        [DbParameterType(NpgsqlDbType.Text)]
        [DefaultValue("ASC")]
        public string SortDir { get; }
    }
}
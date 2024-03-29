using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CoreLibrary;
using DbParametersBuilder.Core;
using DbParametersBuilder.Data;

namespace DbParametersBuilder {
    public class DbParametersDataCollection<T> : IPagingParameters<DbParameter> {
        private readonly IList<IDbParameterData> _parametersData;
        /// <summary>
        /// 0 default, 2 for PagingParameters, 4 for PagingAndSortingParameters
        /// </summary>
        private readonly int _parametersToSkip = 0;
        public virtual IEnumerable<DbParameter> GetParameters(Func<DbParameter> createParameter) {
            var parameters = _parametersData.Select(item => item.Build(createParameter));

            if (BuilderSettings.SkipNullParameters) {
                parameters = parameters.Where(x => x.Value != null);
            }

            return parameters;
        }

        public virtual IEnumerable<DbParameter> GetFilterParameters(Func<DbParameter> createParameter) {
            var parameters = _parametersData.Skip(_parametersToSkip).Select(item => item.Build(createParameter));

            if (BuilderSettings.SkipNullParameters) {
                parameters = parameters.Where(x => x.Value != null);
            }

            return parameters;
        }

        public DbParametersDataCollection(IEnumerable<IDbParameterData> parameters) {
            if (typeof(PagingParameters).IsAssignableFrom(typeof(T)))
                _parametersToSkip += 2;
            if (typeof(SortingPagingParameters).IsAssignableFrom(typeof(T)))
                _parametersToSkip += 2;

            _parametersData = parameters.ToList();
        }
    }
}
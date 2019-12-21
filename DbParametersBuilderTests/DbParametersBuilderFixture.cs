using System;
using System.Data.Common;
using System.Linq;
using DbParametersBuilder;
using Npgsql;

namespace DbParametersBuilderTests {
    public class DbParametersBuilderFixture : IDisposable {
        public DbParametersBuilderFixture() {
            Builder = new DbParametersBuilder<TestParameters>();
        }

        public IDbParametersBuilder<TestParameters> Builder { get; private set; }

        public void Dispose() {
            Builder.Dispose();
        }

        public static readonly Func<DbParameter> CreateNewParameter = () => new NpgsqlParameter();
        public NpgsqlParameter GetNpgsqlParameter(string name = null) {
            var parameters = Builder.BuildParameters().GetParameters(CreateNewParameter);
            return (NpgsqlParameter)parameters?.FirstOrDefault(x => x.ParameterName == name);
        }
    }
}
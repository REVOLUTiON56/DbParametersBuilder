using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CoreLibrary;
using Npgsql;
using NpgsqlTypes;
using Xunit;

namespace DbParametersBuilderTests {
    [Collection("DbParametersBuilder")]
    public class DbParametersBuilderTests : IClassFixture<DbParametersBuilderFixture>, IDisposable {
        private readonly DbParametersBuilderFixture _fixture;

        public DbParametersBuilderTests(DbParametersBuilderFixture fixture) {
            _fixture = fixture;
        }

        public static DbParameter GetDbParameter(IParameters<DbParameter> parameters, string name) {
            return (NpgsqlParameter)parameters.GetParameters(DbParametersBuilderFixture.CreateNewParameter).FirstOrDefault(x => x.ParameterName == name);
        }

        [Fact]
        public void Test_SetValue() {
            _fixture.Builder.SetParameterValue(x => x.IntFilter, 4);

            var parameters = _fixture.Builder.BuildParameters();

            var parameter = GetDbParameter(parameters, "@int_filter");
            Assert.NotNull(parameter.Value);
            Assert.Equal(4, parameter.Value);
        }

        [Fact]
        public void Test_SetValueWithFunc() {
            _fixture.Builder.SetParameterValue(x => x.IntFilter, t => t + 100);
            var parameters = _fixture.Builder.BuildParameters();

            var parameter = GetDbParameter(parameters, "@int_filter");
            Assert.NotNull(parameter.Value);
            Assert.Equal(101, parameter.Value);
        }

        [Fact]
        public void Test_SetValuesFromModel() {
            var model = new TestParameters {
                IntFilter = 5,
            };
            _fixture.Builder.SetParametersFromModel(model);

            var parameters = _fixture.Builder.BuildParameters();

            var parameter = GetDbParameter(parameters, "@int_filter");
            Assert.NotEqual(DBNull.Value, parameter.Value);
            Assert.Equal(5, parameter.Value);
        }

        public void Dispose() {
            _fixture?.Dispose();
        }
    }
}
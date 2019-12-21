using System;
using System.Data;
using System.Data.Common;
using DbParametersBuilder.Data.Shared;
using Npgsql;
using NpgsqlTypes;

namespace DbParametersBuilder.Data {
    public class DbParameterData<T> : IDbParameterData<T> {
        public DbParameterSharedData<T> SharedData { get; }
        public T Value { get; set; }
        public DbParameterData(DbParameterSharedData<T> sharedData) {
            SharedData = sharedData;
            Value = sharedData.DefaultValue;
        }

        public DbParameter Build(Func<DbParameter> func) {
            var parameter = (NpgsqlParameter)func();
            parameter.ParameterName = $"@{SharedData.Name}";
            parameter.Direction = ParameterDirection.Input;
            parameter.SourceColumn = SharedData.Name;
            if (SharedData.Type != NpgsqlDbType.Unknown)
                parameter.NpgsqlDbType = SharedData.Type;

            parameter.Value = ConvertValue() ?? DBNull.Value;
            return parameter;
        }

        protected virtual object ConvertValue() {
            if (Value == null)
                return null;

            return SharedData.Converter.Convert(Value);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               

        public virtual IDbParameterData Clone() {
            return new DbParameterData<T>(SharedData) {
                Value = default
            };
        }

        object IDbParameterData.Value {
            get => Value;
            set => Value = (T)value;
        }

        public void Dispose() {
            Value = SharedData.DefaultValue;
        }
    }
}

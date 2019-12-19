using System;
using System.Reflection;
using CoreLibrary;
using NpgsqlTypes;

namespace DbParametersBuilder.Data.Shared.Interfaces {
    public interface IDbParameterSharedData<out T> {
        PropertyInfo PropertyInfo { get; }
        string Filter { get; }
        NpgsqlDbType Type { get; }
        string Name { get; }
        T DefaultValue { get; }
        Type PropertyType { get; }
        Type ActualType { get; }
    }
}
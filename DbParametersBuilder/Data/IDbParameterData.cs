using System;
using System.Data.Common;
using DbParametersBuilder.Data.Shared;

namespace DbParametersBuilder.Data {

    public interface IDbParameterData {
        DbParameter Build(Func<DbParameter> func);
        void Dispose();
        IDbParameterData Clone();
        object Value { get; set; }
        //ValidationError<Type> GetError<Type>();
    }

    public interface IDbParameterData<T> : IDbParameterData {
        DbParameterSharedData<T> SharedData { get; }
        new T Value { get; set; }
    }
}
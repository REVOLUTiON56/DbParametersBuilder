using System;
using System.Data.Common;
using System.Linq.Expressions;
using CoreLibrary;

namespace DbParametersBuilder {
    public interface IDbParametersBuilder<T> : IDisposable where T : class, new() {
        IParameters<DbParameter> BuildParameters();
        void SetParametersFromModel(T model);
        void SetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value);
        void SetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty, TProperty> func);
        TProperty GetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression);
    }
}
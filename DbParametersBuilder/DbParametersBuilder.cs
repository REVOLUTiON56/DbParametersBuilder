using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using CoreLibrary;
using DbParametersBuilder.Data;

namespace DbParametersBuilder {
    public class DbParametersBuilder<T> : IDbParametersBuilder<T> where T : class, new() {
        protected static readonly Type ObjectType = typeof(T);
        protected readonly IDictionary<PropertyInfo, IDbParameterData> ParametersData;
        #region ctor
        public DbParametersBuilder() {

        }

        public DbParametersBuilder(T model) : this() {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            foreach (var item in ParametersData) {
                ParametersData[item.Key].Value = item.Key.GetValue(model);
            }
        }

        #endregion
        IParameters<DbParameter> IDbParametersBuilder<T>.BuildParameters() {
            return BuildParameters();
        }

        private IDbParameterData<TProperty> GetParameterFromExpression<TProperty>(Expression<Func<T, TProperty>> expression) {
            if (!(expression.Body is MemberExpression memberSelectorExpression))
                throw new InvalidOperationException();

            if (!(memberSelectorExpression.Member is PropertyInfo property))
                throw new InvalidOperationException();

            if (!ParametersData.ContainsKey(property))
                throw new InvalidOperationException();

            if (!(ParametersData[property] is IDbParameterData<TProperty> tParam))
                throw new InvalidOperationException();

            return tParam;
        }

        public virtual void SetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value) {
            var tParam = GetParameterFromExpression(expression);
            tParam.Value = value;
        }

        public virtual void SetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty, TProperty> func) {
            var tParam = GetParameterFromExpression(expression);
            tParam.Value = func(tParam.Value);
        }
        public TProperty GetParameterValue<TProperty>(Expression<Func<T, TProperty>> expression) {
            var tParam = GetParameterFromExpression(expression);
            return tParam.Value;
        }

        public virtual IParameters<DbParameter> BuildParameters() {
            return new DbParametersDataCollection<T>(ParametersData.Values);
        }

        public virtual void Dispose() {
            foreach (var data in ParametersData.Values) {
                data.Dispose();
            }
        }
    }
}
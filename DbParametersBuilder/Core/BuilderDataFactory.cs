using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreLibrary;
using DbParametersBuilder.Data;
using DbParametersBuilder.Data.Shared;

namespace DbParametersBuilder.Core {
    public class BuilderDataFactory {
        internal virtual IDictionary<PropertyInfo, IDbParameterData> InitializeDbParametersData(Type type) {
            if (ReflectionCache.Contains(type))
                return ReflectionCache.Get(type);
            
            var typeAttributes = type.GetCustomAttributes(false);
            var properties =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var parametersData = new ConcurrentDictionary<PropertyInfo, IDbParameterData>();

            if (type.BaseType != null && type.BaseType != typeof(object)) {
                foreach (var p in InitializeDbParametersData(type.BaseType)) {
                    parametersData.GetOrAdd(p.Key, p.Value);
                }
            }

            foreach (var prop in properties)
                parametersData[prop] = CreateData(prop, typeAttributes);
            
            if (!ReflectionCache.Contains(type))
                ReflectionCache.Add(type, parametersData.ToDictionary(x => x.Key, y => y.Value));

            return parametersData;
        }

        public virtual IDbParameterData CreateData(PropertyInfo property, object[] typeAttributes) {
            var propertyType = property.PropertyType;

            if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string)) {
                if (propertyType.IsArray)
                    return CreateArrayParameterData(property, typeAttributes);
                if (propertyType.GetGenericArguments().Length == 1)
                    return CreateEnumerableParameterData(property, typeAttributes);

                throw new NotSupportedException("Filter builder supports only IEnumerables");
            }

            if (propertyType.IsGenericType) {
                if (propertyType.GetGenericTypeDefinition() == typeof(Range<>))
                    return CreateRangeParameterData(property, typeAttributes);
            }

            return CreateBaseParameterData(property, typeAttributes);
        }

        protected virtual IDbParameterData CreateRangeParameterData(PropertyInfo property, object[] typeAttributes) {
            var generalData = Activator.CreateInstance(typeof(DbParameterSharedData<>).MakeGenericType(property.PropertyType),
                new object[] { property, typeAttributes });

            var elementType = property.PropertyType.GetGenericArguments()[0];

            return (IDbParameterData)Activator.CreateInstance(
                typeof(DbParameterData<>).MakeGenericType(property.PropertyType),
                new object[] { generalData });
        }

        protected virtual IDbParameterData CreateEnumerableParameterData(PropertyInfo property, object[] typeAttributes) {

            var elementType = property.PropertyType.GetGenericArguments()[0];

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

            var generalData = Activator.CreateInstance(typeof(DbParameterSharedData<>).MakeGenericType(property.PropertyType),
                new object[] { property, typeAttributes });

            return (IDbParameterData)Activator.CreateInstance(
                typeof(DbParameterData<>).MakeGenericType(property.PropertyType),
                new object[] { generalData });
        }
        protected virtual IDbParameterData CreateArrayParameterData(PropertyInfo property, object[] typeAttributes) {
            var elementType = property.PropertyType.GetElementType();

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

            var generalData = Activator.CreateInstance(typeof(DbParameterSharedData<>).MakeGenericType(property.PropertyType),
                new object[] { property, typeAttributes });

            return (IDbParameterData)Activator.CreateInstance(
                typeof(DbParameterData<>).MakeGenericType(property.PropertyType),
                new object[] { generalData });
        }

        protected virtual IDbParameterData CreateBaseParameterData(PropertyInfo property, object[] typeAttributes) {
            var generalData = Activator.CreateInstance(typeof(DbParameterSharedData<>).MakeGenericType(property.PropertyType),
                new object[] { property, typeAttributes });

            return (IDbParameterData)Activator.CreateInstance(
                typeof(DbParameterData<>).MakeGenericType(property.PropertyType),
                new object[] { generalData });
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbParametersBuilder.Data;

namespace DbParametersBuilder.Core {
    internal static class ReflectionCache {
        private static readonly ConcurrentDictionary<Type, Dictionary<PropertyInfo, IDbParameterData>> Cache
            = new ConcurrentDictionary<Type, Dictionary<PropertyInfo, IDbParameterData>>();

        internal static bool Contains(Type type) {
            return Cache.ContainsKey(type);
        }

        internal static IDictionary<PropertyInfo, IDbParameterData> Get(Type type) {
            return Cache.TryGetValue(type, out var result)
                ? result.ToDictionary(x => x.Key, x => x.Value.Clone())
                : null;
        }
        internal static bool Add(Type type, Dictionary<PropertyInfo, IDbParameterData> data) {
            return Cache.TryAdd(type, data);
        }
    }
}
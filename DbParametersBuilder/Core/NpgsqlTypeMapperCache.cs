using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace DbParametersBuilder.Core {
    internal static class NpgsqlTypeMapperCache {
        private static Lazy<HashSet<Type>> NpgsqlMappedTypes => new Lazy<HashSet<Type>>(InitializeTypesCache);
        private static HashSet<Type> InitializeTypesCache() {
            return new HashSet<Type>(NpgsqlConnection.GlobalTypeMapper.Mappings.SelectMany(x => x.ClrTypes.Where(y => y.IsEnum)));
        }

        public static bool IsMapped(Type enumType) {
            return NpgsqlMappedTypes.Value.Contains(enumType);
        }
    }
}
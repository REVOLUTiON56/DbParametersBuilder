using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CoreLibrary;
using DbParametersBuilder.Attributes;
using DbParametersBuilder.Data.Converters;
using DbParametersBuilder.Data.Shared.Interfaces;
using NpgsqlTypes;

namespace DbParametersBuilder.Data.Shared {
    /// <summary>
    /// Разделяемые данные между всеми параметрами. Они одни для всех параметров
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbParameterSharedData<T> : IDbParameterSharedData<T> {
        public PropertyInfo PropertyInfo { get; }
        public string Filter { get; protected set; }
        public NpgsqlDbType Type { get; protected set; }
        public string Name { get; protected set; }
        public T DefaultValue { get; protected set; }
        public Type PropertyType => typeof(T);
        public Type ActualType => Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;
        public IConverter<T> Converter { get; protected set; }

        protected DbParameterSharedData(PropertyInfo property) {
            PropertyInfo = property ?? throw new ArgumentNullException(nameof(property));

            Filter = property.Name;
            Type = NpgsqlDbType.Unknown;
            Name = property.Name.ToUnderscoreCase();
            DefaultValue = default;
        }

        public DbParameterSharedData(PropertyInfo property, object[] typeAttributes) : this(property) {
            GetDataFromAttributes(property, typeAttributes);
        }

        protected void GetDataFromAttributes(PropertyInfo property, object[] typeAttributes) {
            if (typeAttributes == null)
                throw new ArgumentNullException(nameof(typeAttributes));

            var nameAttribute = property.GetCustomAttribute<DbParameterNameAttribute>();
            var typeAttribute = property.GetCustomAttribute<DbParameterTypeAttribute>();
            var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            var filterNameAttribute = property.GetCustomAttribute<FilterNameAttribute>();
            var converterAttribute = property.GetCustomAttribute<ConverterAttribute>();

            if (converterAttribute != null) {
                Converter = Activator.CreateInstance(converterAttribute.Type.MakeGenericType(ActualType)) as IConverter<T>;
            }
            else {
                Converter = ConvertersFactory.GetConverter<T>(ActualType);
            }

            if (filterNameAttribute != null)
                Filter = filterNameAttribute.FilterName;

            if (typeAttribute != null)
                Type = typeAttribute.Type;

            if (defaultValueAttribute != null) {
                try {
                    DefaultValue = (T)Convert.ChangeType(defaultValueAttribute.Value, typeof(T));
                }
                catch {
                    DefaultValue = default;
                }
            }

            if (nameAttribute != null)
                Name = nameAttribute.Name;

            //GeneralAttributes

            if (!typeAttributes.Any())
                return;

            NpgSqlDbParametersPrefixAttribute prefixAttribute = null;

            foreach (var ta in typeAttributes) {
                if (ta is NpgSqlDbParametersPrefixAttribute attribute)
                    prefixAttribute = attribute;
            }

            if (nameAttribute == null && prefixAttribute != null)
                Name = string.Join("_", prefixAttribute.Prefix, Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreLibrary;
using DbParametersBuilder.Attributes;
using NpgsqlTypes;
using Xunit.Sdk;

namespace DbParametersBuilderTests {
    public class TestParameters {
        public string StringFilter { get; set; }
        [DefaultValue(1)]
        public int IntFilter { get; set; }
        public decimal? DecimalFilter { get; set; }
        public double? DoubleFilter { get; set; }
        public bool? BoolFilter { get; set; }
        [DbParameterType(NpgsqlDbType.Range | NpgsqlDbType.Date)]
        [Range("DateRangeFilterStart", "DateRangeFilterEnd")]
        public Range<DateTime> DateRangeFilter { get; set; }

        [FilterName("ShortRangeFilterNullable")]
        public Range<short>? ShortRangeFilterNullable { get; set; }
        public long[] LongArrayFilter { get; set;  }
        public TestEnum? EnumFilter { get; set; }
        public List<TestEnum> EnumListFilter { get; set; }
        public NpgsqlTestEnum NpgsqlEnumFilter { get; set; }
        public IList<NpgsqlTestEnum> NpgsqlEnumListFilter { get; set; }
        public TestClass TestClassFilter { get; set; }
    }

    public enum TestEnum : byte {
        zero = 0,
        one = 1,
        two = 2,
        three = 3,
        four = 4,
        five = 5,
    }


    public enum NpgsqlTestEnum {
        zero = 0,
        one = 1,
        two = 2,
        three = 3,
        four = 4,
        five = 5,
    }
}
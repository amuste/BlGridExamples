using BlGrid.Api.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace BlGrid.Api.Tests
{
    public static class DbSeeder
    {
        public static void Feed(ITestContext context)
        {
            context.Entities.Add(new TestEntity
            {
                String = "Paco",
                Int = 10,
                Long = 10,
                Date = new System.DateTime(2020,01,01),
                Bool = true,
                BoolNullable = true
            });

            context.Entities.Add(new TestEntity
            {
                String = "Adrian",
                Int = 20,
                Long = 20,
                Date = new System.DateTime(2019, 01, 01),
                Bool = true,
                BoolNullable = true
            });

            context.Entities.Add(new TestEntity
            {
                String = "Paco the great",
                Int = 30,
                Long = 30,
                Float = 3.25f,
                Double = 5.25f,
                Date = new System.DateTime(2018, 01, 01, 01,02,03),
                Bool = false,
                BoolNullable = false
            });

            context.Entities.Add(new TestEntity
            {
                String = "Adrian the great",
                Int = 40,
                IntNullable = int.MaxValue,
                UInt = uint.MaxValue,
                UIntNullable = uint.MaxValue,
                Long = long.MinValue,
                LongNullable = long.MaxValue,
                ULong = ulong.MaxValue,
                ULongNullable = ulong.MaxValue,
                Short = short.MinValue,
                ShortNullable = short.MaxValue,
                UShort = ushort.MaxValue,
                UShortNullable = ushort.MaxValue,
                Decimal = decimal.MaxValue,
                DecimalNullable = decimal.MaxValue,
                Float = float.MaxValue,
                FloatNullable = float.MaxValue,
                Double = double.MaxValue,
                DoubleNullable = double.MaxValue,
                Date = new System.DateTime(2017, 01, 01),
                DateNullable = new System.DateTime(2020, 01, 01),
                Bool = true,
            });

        }

        public static List<TestEntity> FeedList()
        {
            var list = new List<TestEntity>();

            list.Add(new TestEntity
            {
                String = "Paco",
                Int = 10,
                Long = 10,
            });

            list.Add(new TestEntity
            {
                String = "Adrian",
                Int = 20,
                Long = 20,
            });

            list.Add(new TestEntity
            {
                String = "Paco the great",
                Int = 30,
                Long = 30,
                Float = 3.25f
            });

            list.Add(new TestEntity
            {
                String = "Adrian the great",
                Int = 40,
                IntNullable = int.MaxValue,
                UInt = uint.MaxValue,
                UIntNullable = uint.MaxValue,
                Long = long.MinValue,
                LongNullable = long.MaxValue,
                ULong = ulong.MaxValue,
                ULongNullable = ulong.MaxValue,
                Short = short.MinValue,
                ShortNullable = short.MaxValue,
                UShort = ushort.MaxValue,
                UShortNullable = ushort.MaxValue,
                Decimal = decimal.MaxValue,
                DecimalNullable = decimal.MaxValue,
                Float = float.MaxValue,
                FloatNullable = float.MaxValue

            });

            return list;
        }
    }
}


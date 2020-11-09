using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlGrid.Api.Tests
{
    public class TestEntity
    {
        [Key]
        public int Id { get; set; }

        public string String { get; set; }

        public int Int { get; set; }

        public uint UInt { get; set; }

        public int? IntNullable { get; set; }

        public uint? UIntNullable { get; set; }

        public long Long { get; set; }

        public long? LongNullable { get; set; }

        public ulong ULong { get; set; }

        public ulong? ULongNullable { get; set; }

        public short Short { get; set; }

        public short? ShortNullable { get; set; }

        public ushort UShort { get; set; }

        public ushort? UShortNullable { get; set; }

        public decimal Decimal { get; set; }

        public decimal? DecimalNullable { get; set; }

        public float Float { get; set; }

        public float? FloatNullable { get; set; }

        public double Double { get; set; }

        public double? DoubleNullable { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateNullable { get; set; }

        public bool Bool { get; set; }

        public bool? BoolNullable { get; set; }
    }
}

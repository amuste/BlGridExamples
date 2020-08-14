using System;

// ReSharper disable BuiltInTypeReferenceStyle

namespace BlGrid.Api.Infrastructure
{
    public static class TypeExtensions
    {
        public static bool IsPrimitive(this Type type)
        {
            return type == typeof(String) ||
                    type == typeof(Decimal) ||
                    type == typeof(Int16) ||
                    type == typeof(Int32) ||
                    type == typeof(Int64) ||
                    type == typeof(Boolean);
        }

        public static bool IsNumeric(this Type type)
        {
            return type.IsPrimitive() ||
                    type == typeof(int) ||
                    type == typeof(int?) ||
                    type == typeof(decimal) ||
                    type == typeof(decimal?) ||
                    type == typeof(float) ||
                    type == typeof(float?) ||
                    type == typeof(double) ||
                    type == typeof(double?);
        }

        public static bool IsString(this Type type)
        {
            return  type == typeof(string) ||
                    type == typeof(string);
        }

        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime) ||
                    type == typeof(DateTime?);
        }
    }
}

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
            var baseType = type;
            var notNUllableType = Nullable.GetUnderlyingType(type);

            if (notNUllableType != null)
                baseType = notNUllableType;

            switch (Type.GetTypeCode(baseType))
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
            }

            return false;
        }

        public static bool IsBoolean(this Type type)
        {
            var baseType = type;
            var notNUllableType = Nullable.GetUnderlyingType(type);

            if (notNUllableType != null)
                baseType = notNUllableType;

            switch (Type.GetTypeCode(baseType))
            {
                case TypeCode.Boolean:
                    return true;
            }

            return false;
        }

        public static bool IsString(this Type type)
        {
            return type == typeof(string) ||
                    type == typeof(string);
        }

        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime) ||
                    type == typeof(DateTime?);
        }
    }
}

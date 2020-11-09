using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class NumberFilter<TEntity> : IBaseFilter<TEntity>
    {
        public FilterPredicate GetPredicateExpression(
            IQueryable<TEntity> query, 
            AdvancedFilterModel filter, 
            ParameterExpression accessExpression)
        {
            var resultPredicate = GetSingleFilterPredicateExpression(filter, accessExpression);

            var filterPredicate = new FilterPredicate()
            {
                Predicate = resultPredicate,
                OperatorType = filter.FilterLinkCondition
            };

            return filterPredicate;
        }

        public Expression GetSingleFilterPredicateExpression(AdvancedFilterModel filter, ParameterExpression accessExpression)
        {
            var type = typeof(TEntity);

            var property = type.GetProperty(filter.Column);

            if (property == null) throw new NullReferenceException(nameof(property));

            Expression predicate;
            Expression aditionalPredicate = null;
           
            if(property.PropertyType.IsNumeric())
            {
                predicate = GetOperationExpression(filter.Operator, filter.Value, filter.Column, accessExpression);

                if (filter.AdditionalOperator != FilterOperator.None && !string.IsNullOrEmpty(filter.AdditionalValue))
                {
                    aditionalPredicate = GetOperationExpression(filter.AdditionalOperator, filter.AdditionalValue, filter.Column, accessExpression);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The numeric filter over column {filter.Column} with value {filter.Value} can't be parsed");
            }

            var resultPredicate = filter.Condition switch
            {
                FilterCondition.Or when aditionalPredicate != null => Expression.Or(predicate, aditionalPredicate),

                FilterCondition.And when aditionalPredicate != null => Expression.And(predicate, aditionalPredicate),

                FilterCondition.None => predicate,

                _ => throw new ArgumentOutOfRangeException()
            };

            return resultPredicate;
        }

        private Expression GetOperationExpression(
            FilterOperator filterOperator, 
            string value, 
            string propertyName, 
            ParameterExpression accessExpression)
        {
            var type = typeof(TEntity);

            var property = type.GetProperty(propertyName);

            if (property == null) throw new NullReferenceException(nameof(property));

            var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

            var left = propertyExpression;
            var right = GetRightExpresion(property, value);
            

            var predicate = filterOperator switch
            {
                FilterOperator.Equals => Expression.Equal(left, right),

                FilterOperator.NotEquals => Expression.NotEqual(left, right),

                FilterOperator.GreaterThan => Expression.GreaterThan(left, right),

                FilterOperator.LessThan => Expression.LessThan(left, right),

                _ => throw new ArgumentOutOfRangeException($"The operator for column \"{propertyName}\" is not supported")
            };
 
            return predicate;
        }

        private Expression GetRightExpresion(PropertyInfo property, string value)
        {
            value = value != null ? value.Replace(',', '.') : null;

            if (Nullable.GetUnderlyingType(property.PropertyType) != null)
            {
                return Type.GetTypeCode(Nullable.GetUnderlyingType(property.PropertyType)) switch
                {
                    TypeCode.UInt16 => Expression.Convert(Expression.Constant(ushort.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal1) ? tempVal1 : (ushort?)null), property.PropertyType),
                    TypeCode.UInt32 => Expression.Convert(Expression.Constant(uint.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal2) ? tempVal2 : (uint?)null), property.PropertyType),
                    TypeCode.UInt64 => Expression.Convert(Expression.Constant(ulong.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal3) ? tempVal3 : (ulong?)null), property.PropertyType),
                    TypeCode.Int16 => Expression.Convert(Expression.Constant(short.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal4) ? tempVal4 : (short?)null), property.PropertyType),
                    TypeCode.Int32 => Expression.Convert(Expression.Constant(int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal5) ? tempVal5 : (int?)null), property.PropertyType),
                    TypeCode.Int64 => Expression.Convert(Expression.Constant(long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal6) ? tempVal6 : (long?)null), property.PropertyType),
                    TypeCode.Decimal => Expression.Convert(Expression.Constant(decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal7) ? tempVal7 : (decimal?)null), property.PropertyType),
                    TypeCode.Single => Expression.Convert(Expression.Constant(float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal8) ? tempVal8 : (float?)null), property.PropertyType),
                    TypeCode.Double => Expression.Convert(Expression.Constant(double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempVal8) ? tempVal8 : (double?)null), property.PropertyType),
                    _ => throw new ArgumentOutOfRangeException($"Property {property.Name} does not have a vlid numeric type")
                };
            }

            return Type.GetTypeCode(property.PropertyType) switch
            {
                TypeCode.UInt16 => Expression.Constant(ushort.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.UInt32 => Expression.Constant(uint.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.UInt64 => Expression.Constant(ulong.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Int16 => Expression.Constant(short.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Int32 => Expression.Constant(int.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Int64 => Expression.Constant(long.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Decimal => Expression.Constant(decimal.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Single => Expression.Constant(float.Parse(value, CultureInfo.InvariantCulture)),
                TypeCode.Double => Expression.Constant(double.Parse(value, CultureInfo.InvariantCulture)),
                _ => throw new ArgumentOutOfRangeException($"Property {property.Name} does not have a vlid numeric type")
            };
        }
    }
}
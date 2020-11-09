using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class BooleanFilter<TEntity> : IBaseFilter<TEntity>
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

            if (property.PropertyType.IsBoolean())
            {
                predicate = GetOperationExpression(filter.Operator, filter.Value, filter.Column, accessExpression);

                if (filter.AdditionalOperator != FilterOperator.None && !string.IsNullOrEmpty(filter.AdditionalValue))
                {
                    aditionalPredicate = GetOperationExpression(
                        filter.AdditionalOperator, 
                        filter.AdditionalValue, 
                        filter.Column, 
                        accessExpression);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The boolean filter over column {filter.Column} with value {filter.Value} can't be parsed");
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
                _ => throw new ArgumentOutOfRangeException($"The operator for column \"{propertyName}\" is not supported")
            };

            return predicate;
        }

        private Expression GetRightExpresion(PropertyInfo property, string value)
        {
            if (Nullable.GetUnderlyingType(property.PropertyType) != null)
            {
                return Expression.Convert(Expression.Constant(bool.TryParse(value, out var tempVal1) ? tempVal1 : (bool?)null), property.PropertyType);
            }

            return Expression.Constant(bool.Parse(value));
        }
    }
}
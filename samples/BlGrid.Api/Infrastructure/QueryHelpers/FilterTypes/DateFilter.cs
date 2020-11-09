using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class DateFilter<TEntity> : IBaseFilter<TEntity>
    {
        public FilterPredicate GetPredicateExpression(IQueryable<TEntity> query, AdvancedFilterModel filter, ParameterExpression accessExpression)
        {
            var resultPredicate = GetSingleFilterPredicateExpression(filter, accessExpression);

            var filterPredicate = new FilterPredicate()
            {
                Predicate = resultPredicate,
                OperatorType = filter.Condition
            };

            return filterPredicate;
        }

        public Expression GetSingleFilterPredicateExpression(
             AdvancedFilterModel filter,
             ParameterExpression accessExpression)
        {
            var type = typeof(TEntity);
            var propertyType = type.GetProperty(filter.Column);

            var propertyExpression = Expression.MakeMemberAccess(accessExpression, propertyType ?? throw new InvalidOperationException());
           

            if (propertyType == null) throw new NullReferenceException(nameof(propertyType));

            var dateValue = filter.Value != null ? 
                DateTime.ParseExact(filter.Value, filter.DateFormat, CultureInfo.InvariantCulture) :
                (DateTime?)null; ;

            var dateAdditional = filter.AdditionalValue != null ? 
                DateTime.ParseExact(filter.AdditionalValue, filter.DateFormat, CultureInfo.InvariantCulture) : 
                (DateTime?)null;

            var filterExpression = GetOperationExpression(filter, propertyExpression, dateValue, dateAdditional);

            return filterExpression;
        }

        private static Expression GetOperationExpression(
            AdvancedFilterModel filter,
            Expression accessExpression,
            DateTime? value,
            DateTime? valueAdditional = null)
        {
            Expression predicate;

            var type = typeof(TEntity);
            var propertyType = type.GetProperty(filter.Column);

            Expression constant;

            Expression constantAdditional;

            if (Nullable.GetUnderlyingType(propertyType.PropertyType) != null)
            {
                constant = Expression.Convert(Expression.Constant(value != null ? value : (DateTime?)null), propertyType.PropertyType);
                constantAdditional = Expression.Convert(
                    Expression.Constant(valueAdditional != null ? 
                        valueAdditional : 
                        (DateTime?)null), propertyType.PropertyType);
            }
            else
            {
                constant = Expression.Constant(value);
                constantAdditional = Expression.Constant(valueAdditional);
            }

            switch (filter.Operator)
            {

                case FilterOperator.Equals:
                    predicate = Expression.Equal(accessExpression, constant);
                    break;

                case FilterOperator.NotEquals:
                    predicate = Expression.NotEqual(accessExpression, constant);
                    break;

                case FilterOperator.GreaterThan:
                    predicate = Expression.GreaterThanOrEqual(accessExpression, constant);
                    break;

                case FilterOperator.LessThan:
                    predicate = Expression.LessThanOrEqual(accessExpression, constant);
                    break;

                case FilterOperator.Range:
                    var predicateLess = Expression.GreaterThanOrEqual(accessExpression, constant);
                    var predicateGreat = Expression.LessThanOrEqual(accessExpression, constantAdditional);
                    predicate = Expression.And(predicateLess, predicateGreat);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return predicate;
        }
    }
}
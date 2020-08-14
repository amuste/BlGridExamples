using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class DateFilter<TEntity> : IBaseFilter<TEntity>
    {
        public IQueryable<TEntity> BuildFilterExpression(IQueryable<TEntity> query, List<FilterPredicate> filterPredicates, ParameterExpression accessExpression)
        {

            var mainPredicate = filterPredicates.First(p => p.OperatorType == FilterCondition.None).Predicate;
            var predicates = filterPredicates.Where(p => p.OperatorType != FilterCondition.None);

            foreach (var predicate in predicates)
            {
                if (predicate.OperatorType == FilterCondition.Or)
                {
                    mainPredicate = Expression.Or(mainPredicate, predicate.Predicate);
                }
                else
                {
                    var queraExpresion1 = Expression.Lambda<Func<TEntity, bool>>(predicate.Predicate, accessExpression);
                    query = query.Where(queraExpresion1);
                }
            }

            var queraExpresion = Expression.Lambda<Func<TEntity, bool>>(mainPredicate, accessExpression);

            return query.Where(queraExpresion);
        }

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

            var property = type.GetProperty(filter.Column);
            var propertyExpression = Expression.MakeMemberAccess(accessExpression, property ?? throw new InvalidOperationException());

            if (propertyType == null) throw new NullReferenceException(nameof(propertyType));

            var dateProperty = propertyType.PropertyType.GetProperty("Date");
            if (dateProperty == null) throw new NullReferenceException(nameof(dateProperty));

            var dateExpression = Expression.MakeMemberAccess(propertyExpression, dateProperty);

            var dateValue = DateTime.ParseExact(filter.Value, filter.DateFormat, CultureInfo.InvariantCulture);

            var dateAdditional = filter.AdditionalValue != null ? DateTime.ParseExact(filter.AdditionalValue, filter.DateFormat, CultureInfo.InvariantCulture)
                                                                      : (DateTime?)null;

            var filterExpression = GetOperationExpression(filter, dateExpression, dateValue, dateAdditional);

            return filterExpression;
        }

        private static Expression GetOperationExpression(
            AdvancedFilterModel filter,
            Expression accessExpression,
            DateTime value,
            DateTime? valueAdditional = null)
        {
            Expression predicate;

            switch (filter.Operator)
            {

                case FilterOperator.Equals:
                    predicate = Expression.Equal(accessExpression, Expression.Constant(value));
                    break;

                case FilterOperator.NotEquals:
                    predicate = Expression.NotEqual(accessExpression, Expression.Constant(value));
                    break;

                case FilterOperator.GreaterThan:
                    predicate = Expression.GreaterThanOrEqual(accessExpression, Expression.Constant(value));
                    break;

                case FilterOperator.LessThan:
                    predicate = Expression.LessThanOrEqual(accessExpression, Expression.Constant(value));
                    break;

                case FilterOperator.Range:
                    var predicateLess = Expression.GreaterThanOrEqual(accessExpression, Expression.Constant(value));
                    var predicateGreat = Expression.LessThanOrEqual(accessExpression, Expression.Constant(valueAdditional));
                    predicate = Expression.And(predicateLess, predicateGreat);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return predicate;
        }
    }
}
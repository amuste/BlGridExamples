using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class NumberFilter<TEntity> : IBaseFilter<TEntity>
    {
        public IQueryable<TEntity> BuildFilterExpression(IQueryable<TEntity> query, List<FilterPredicate> filterPredicates, ParameterExpression accessExpression)
        {

            var mainPredicate = filterPredicates.First().Predicate;

            if (mainPredicate == null) return query;

            var predicates = filterPredicates.Select(e => e).Skip(1);

            foreach (var predicate in predicates)
            {
                if (predicate.OperatorType == FilterCondition.Or)
                {
                    mainPredicate = Expression.Or(mainPredicate, predicate.Predicate);
                }
                else
                {
                    var queryExpresion1 = Expression.Lambda<Func<TEntity, bool>>(predicate.Predicate, accessExpression);
                    query = query.Where(queryExpresion1);
                }
            }

            var queryExpresion = Expression.Lambda<Func<TEntity, bool>>(mainPredicate, accessExpression);

            return query.Where(queryExpresion);
        }

        public FilterPredicate GetPredicateExpression(IQueryable<TEntity> query, AdvancedFilterModel filter, ParameterExpression accessExpression)
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
                predicate = GetOperationExpression(filter.Operator, int.Parse(filter.Value), filter.Column, accessExpression, filter);

                if (filter.AdditionalOperator != FilterOperator.None && !string.IsNullOrEmpty(filter.AdditionalValue))
                {
                    aditionalPredicate = GetOperationExpression(filter.AdditionalOperator, int.Parse(filter.AdditionalValue), filter.Column, accessExpression, filter);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
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

        private Expression GetOperationExpression(FilterOperator filterOperator, int value, string propertyName, ParameterExpression accessExpression, AdvancedFilterModel filter)
        {
            var type = typeof(TEntity);

            var property = type.GetProperty(propertyName);

            if (property == null) throw new NullReferenceException(nameof(property));

            var mytype = property.PropertyType.ToString() switch
            {
                "System.Nullable`1[System.Int16]" => typeof(int),

                "System.Nullable`1[System.Int32]" => typeof(int),

                "System.Nullable`1[System.Int64]" => typeof(int),

                "System.Nullable`1[System.Decimal]" => typeof(decimal),

                "System.Nullable`1[System.Double]" => typeof(double),

                "System.Nullable`1[System.Float]" => typeof(float),

                _ => throw new ArgumentOutOfRangeException()
            };

            var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

            var left = Expression.Convert(propertyExpression, mytype);

            var right = Expression.Constant(value);

            var predicate = filterOperator switch
            {
                FilterOperator.Equals => Expression.Equal(left, right),

                FilterOperator.NotEquals => Expression.NotEqual(left, right),

                FilterOperator.GreaterThan => Expression.GreaterThan(left, right),

                FilterOperator.LessThan => Expression.LessThan(left, right),

                _ => throw new ArgumentOutOfRangeException()
            };

            return predicate;
        }
    }
}
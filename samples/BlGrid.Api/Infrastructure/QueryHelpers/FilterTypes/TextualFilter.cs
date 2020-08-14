using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class TextualFilter<TEntity> : IBaseFilter<TEntity>
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

            var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

            Expression predicate;
            Expression aditionalPredicate = null;

            if (property.PropertyType.IsString())
            {
                predicate = GetOperationExpression(filter.Operator, filter.Value, propertyExpression);

                if (filter.AdditionalOperator != FilterOperator.None && !string.IsNullOrEmpty(filter.AdditionalValue))
                {
                    aditionalPredicate = GetOperationExpression(filter.AdditionalOperator, filter.AdditionalValue, propertyExpression);
                }
            }
            else
            {
                var toString = type.GetMethod("ToString");
                var stringExpression = Expression.Call(propertyExpression, toString ?? throw new InvalidOperationException());

                predicate = GetOperationExpression(filter.AdditionalOperator, filter.AdditionalValue, stringExpression);

                if (filter.AdditionalOperator != FilterOperator.None && !string.IsNullOrEmpty(filter.AdditionalValue))
                {
                    aditionalPredicate = GetOperationExpression(filter.AdditionalOperator, filter.AdditionalValue, stringExpression);
                }
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

        private Expression GetOperationExpression(FilterOperator filterOperator, string value, Expression propertyExpression)
        {
            var predicate = filterOperator switch
            {
                FilterOperator.Contains => FilterByFunction(propertyExpression, "Contains", value),

                FilterOperator.NotContains => FilterByFunctionNegation(propertyExpression, "Contains", value),

                FilterOperator.Equals => FilterByFunction(propertyExpression, "Equals", value),

                FilterOperator.NotEquals => FilterByFunctionNegation(propertyExpression, "Equals", value),

                FilterOperator.StartsWith => FilterByFunction(propertyExpression, "StartsWith", value),

                FilterOperator.EndsWith => FilterByFunction(propertyExpression, "EndsWith", value),

                _ => throw new ArgumentOutOfRangeException()
            };

            return predicate;
        }

        private Expression FilterByFunction(Expression propertyExpression, string function, string value)
        {
            var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == function);

            if (filterMethod == null) throw new NullReferenceException(nameof(filterMethod));

            return Expression.Call(propertyExpression, filterMethod, Expression.Constant(value));
        }

        private Expression FilterByFunctionNegation(Expression propertyExpression, string function, string value)
        {
            var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == function);

            if (filterMethod == null) throw new NullReferenceException(nameof(filterMethod));

            return Expression.Not(Expression.Call(propertyExpression, filterMethod, Expression.Constant(value)));
        }
    }
}
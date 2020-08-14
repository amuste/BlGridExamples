using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;
using static System.Boolean;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal class BooleanFilter<TEntity> : IBaseFilter<TEntity>
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
            var predicate = FilterBooleanByFunction(filter.Column, filter.Value, accessExpression);

            return predicate;
        }

        private Expression FilterBooleanByFunction(string propertyName, string value, ParameterExpression accessExpression)
        {

            var type = typeof(TEntity);

            var property = type.GetProperty(propertyName);

            if (property == null) throw new NullReferenceException(nameof(property));

            Expression propertyExpression;

            var booleanVAlue = Parse(value);

            if (booleanVAlue)
            {
                propertyExpression = Expression.MakeMemberAccess(accessExpression, property);
            }
            else
            {
                propertyExpression = Expression.Not(Expression.MakeMemberAccess(accessExpression, property));
            }

            return propertyExpression;
        }
    }
}
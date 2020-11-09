using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers
{
    public class FilterHelper<TEntity>
    {
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query, SearchModel searchModel)
        {
            if (searchModel.AdvancedFilterModels == null || searchModel.AdvancedFilterModels.Count <= 0) return query;

            var accessExpresion = Expression.Parameter(typeof(TEntity), "x");

            var filterPredicates = new List<FilterPredicate>();

            var textualFilter = new TextualFilter<TEntity>();
            var dateFilter = new DateFilter<TEntity>();
            var booleanFilter = new BooleanFilter<TEntity>();
            var numberFilter = new NumberFilter<TEntity>();

            foreach (var filter in searchModel.AdvancedFilterModels)
            {
                switch (filter.Type)
                {
                    case CellDataType.Date:
                        var filterPredicate = dateFilter.GetPredicateExpression(query, filter, accessExpresion);
                        filterPredicates.Add(filterPredicate);
                        break;

                    case CellDataType.Text:
                        var filterPredicate1 = textualFilter.GetPredicateExpression(query, filter, accessExpresion);
                        filterPredicates.Add(filterPredicate1);
                        break;

                    case CellDataType.Number:
                        var filterPredicate2 = numberFilter.GetPredicateExpression(query, filter, accessExpresion);
                        filterPredicates.Add(filterPredicate2);
                        break;

                    case CellDataType.Bool:
                        var filterPredicate3 = booleanFilter.GetPredicateExpression(query, filter, accessExpresion);
                        filterPredicates.Add(filterPredicate3);
                        break;

                    case CellDataType.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            query = BuildFilterExpression(query, filterPredicates, accessExpresion);

            return query;
        }

        private IQueryable<TEntity> BuildFilterExpression(
            IQueryable<TEntity> query, 
            List<FilterPredicate> filterPredicates, 
            ParameterExpression accessExpression)
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
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers
{
    public class SortHelper<TEntity>
    {
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query, SearchModel searchModel)
        {
            var isAscendingSort = searchModel.SortModel.Order == SortOrder.Ascending;

            if (!string.IsNullOrEmpty(searchModel.SortModel.ColumnName) && searchModel.SortModel.Order != SortOrder.None)
                return OrderBy(query, searchModel.SortModel.ColumnName, isAscendingSort);

            return query;
        }

        private static IQueryable<TEntity> OrderBy(IQueryable source, string ordering, bool isAscending = true)
        {
            var type = typeof(TEntity);
            var property = type.GetProperty(ordering);

            //E1 = x 
            var expression1 = Expression.Parameter(type, "x");

            //E2 = E1.PROPERTY
            var expression2 = Expression.MakeMemberAccess(expression1, property ?? throw new InvalidOperationException());

            //E3 = E1 => E2
            var expression3 = Expression.Lambda(expression2, expression1);

            var sortType = isAscending ? "OrderBy" : "OrderByDescending";

            var resultExp = Expression.Call(typeof(Queryable), sortType, new[] { type, property.PropertyType }, source.Expression, Expression.Quote(expression3));
            return source.Provider.CreateQuery<TEntity>(resultExp);
        }
    }
}
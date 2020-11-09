using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterTypes
{
    internal interface IBaseFilter<TEntity>
    {
        FilterPredicate GetPredicateExpression(
          IQueryable<TEntity> query,
          AdvancedFilterModel filter,
          ParameterExpression accessExpression
        );

        Expression GetSingleFilterPredicateExpression(
            AdvancedFilterModel filter,
            ParameterExpression accessExpression
            );
    }
}

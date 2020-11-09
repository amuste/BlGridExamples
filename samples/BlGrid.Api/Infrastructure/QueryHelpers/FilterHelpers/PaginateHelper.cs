using System.Linq;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers
{
    public class PaginateHelper<TEntity>
    {
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query, SearchModel searchModel)
        {
            if (searchModel.PaginationModel.CurrentPage.HasValue && searchModel.PaginationModel.PageSize.HasValue)
                return query
                    .Skip((searchModel.PaginationModel.CurrentPage.Value - 1) * searchModel.PaginationModel.PageSize.Value)
                    .Take(searchModel.PaginationModel.PageSize.Value);

            return query;
        }
    }
}

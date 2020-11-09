using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlGrid.Api.Infrastructure.QueryHelpers;
using BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BlGrid.Api.Infrastructure
{
    public class TestRepository<Entity> where Entity : class
    {
        private readonly ITestContext _dbContext;

        public TestRepository(ITestContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Entity>> Search(SearchModel searchModel)
        {
            var entities = _dbContext.GetDbContext().Set<Entity>().AsQueryable();

            if (searchModel?.AdvancedFilterModels != null) entities = new FilterHelper<Entity>().Apply(entities, searchModel);

            if (searchModel?.SortModel != null) entities = new SortHelper<Entity>().Apply(entities, searchModel);

            var query = from entity in entities select entity;

            if (searchModel?.PaginationModel != null)
            {
                var pageNumber = searchModel.PaginationModel.CurrentPage <= 0 ? 1 : searchModel.PaginationModel.CurrentPage;

                var pageSize = searchModel.PaginationModel.PageSize;

                if (pageNumber != null && pageSize != null) query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var persons = await query.ToListAsync();

            return persons;
        }

    }
}

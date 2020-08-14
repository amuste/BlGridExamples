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
    public class PersonRepository : IPersonRepository
    {
        private readonly IBlGridDbContext _dbContext;

        public PersonRepository(IBlGridDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Person>> GetPersons()
        {
            var persons = await (from person in _dbContext.Persons
                                 select person).ToListAsync();
            return persons;
        }

        public async Task<List<Person>> SearchPersons(SearchModel searchModel)
        {
            var entities = _dbContext.GetDbContext().Set<Person>().AsQueryable();

            if(searchModel?.AdvancedFilterModels != null) entities = new FilterHelper<Person>().Apply(entities, searchModel);

            if (searchModel?.SortModel != null) entities = new SortHelper<Person>().Apply(entities, searchModel);

            var query = from person in entities select person;

            if (searchModel?.PaginationModel != null)
            {
                var pageNumber = searchModel.PaginationModel.CurrentPage <= 0 ? 1 : searchModel.PaginationModel.CurrentPage;

                var pageSize = searchModel.PaginationModel.PageSize;

                if (pageNumber != null && pageSize != null) query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var persons = await query.ToListAsync();

            return persons;
        }

        public async Task<int> CountPersons(SearchModel searchModel)
        {
            var entities = _dbContext.GetDbContext().Set<Person>().AsQueryable();

            if (searchModel?.AdvancedFilterModels != null) entities = new FilterHelper<Person>().Apply(entities, searchModel);

            if (searchModel?.AdvancedFilterModels != null) entities = new SortHelper<Person>().Apply(entities, searchModel);

            var query = from person in entities select person;

            var persons = await query.CountAsync();

            return persons;
        }

        public async Task AddPersons(List<Person> persons)
        {
            await using var dbContextTransaction = _dbContext.GetDatabase().BeginTransaction();

            try
            {
                await _dbContext.GetDbContext().BulkInsertAsync(persons);

                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                dbContextTransaction.Rollback();
                throw;
            }
        }
    }
}

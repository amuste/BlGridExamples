using BlGrid.Api.Infrastructure;
using BlGrid.Api.Infrastructure.QueryHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;

namespace BlGrid.Api.Tests
{
    [TestClass]
    public class BlGridRepositorySortTextualTests
    {
        TestDBContext Context;
        TestRepository<TestEntity> _repository;


        [TestCleanup]
        public void Cleanup()
        {
            Context.Dispose();
        }

        [TestInitialize]
        public void Setup()
        {
            var options = SqliteInMemory
                .CreateOptions<TestDBContext>();

            Context = new TestDBContext(options);
            Context.Database.EnsureCreated();

            DbSeeder.Feed(Context);

            Context.SaveChanges();

            _repository = new TestRepository<TestEntity>(Context);
        }

        [TestMethod]
        public async Task Sort_Textual_Ascending()
        {
            await TestSearch("Adrian", SortOrder.Ascending);
        }

        [TestMethod]
        public async Task Sort_Textual_Descending()
        {
            await TestSearch("Paco the great", SortOrder.Descending);
        }

        [TestMethod]
        public async Task Sort_Textual_None()
        {
            await TestSearch("Paco", SortOrder.None);
        }

        private async Task TestSearch(string value, SortOrder sortOrder)
        {
            var searchModel = BuildSearchModel(sortOrder);
            var entities = await _repository.Search(searchModel);
            entities.First().String.ShouldBe(value);
        }

        private SearchModel BuildSearchModel(SortOrder sortOrder)
        {
            return new SearchModel
            {
                SortModel = new SortModel
                {
                    ColumnName = "String",
                    Order = sortOrder
                }
            };
        }
    }
}

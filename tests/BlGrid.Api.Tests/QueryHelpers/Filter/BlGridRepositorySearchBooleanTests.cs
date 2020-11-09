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
    public class BlGridRepositorySearchBooleanTests
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
            var options = EfInMemory
                .CreateOptions<TestDBContext>();

            Context = new TestDBContext(options);
            Context.Database.EnsureCreated();

            DbSeeder.Feed(Context);

            Context.SaveChanges();

            _repository = new TestRepository<TestEntity>(Context);
        }


        [TestMethod]
        public async Task Search_Bool_Equals_False()
        {
            await BooleanSearch("false", FilterOperator.Equals,"Bool", 1);
        }

        [TestMethod]
        public async Task Search_Bool_Equals_True()
        {
            await BooleanSearch("true", FilterOperator.Equals, "Bool", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task Search_Bool_Equals_NotParsed()
        {
            await BooleanSearch("truesss", FilterOperator.Equals, "Bool", 3);
        }

        [TestMethod]
        public async Task Search_Bool_Equals_Nullable_False()
        {
            await BooleanSearch("false", FilterOperator.Equals, "BoolNullable", 1);
        }

        [TestMethod]
        public async Task Search_Bool_Equals_Nullable_True()
        {
            await BooleanSearch("true", FilterOperator.Equals, "BoolNullable", 2);
        }

        [TestMethod]
        public async Task Search_Bool_Equals_Nullable_Null()
        {
            await BooleanSearch(null, FilterOperator.Equals, "BoolNullable", 1);
        }

        private async Task BooleanSearch(string value, FilterOperator operato, string column,  int total)
        {
            var searchModel = BuildSearchModel(value, operato, column);
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(total);
        }

        private SearchModel BuildSearchModel(string value, FilterOperator operato, string column)
        {
            return new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = column,
                        Type =  CellDataType.Bool,
                        Value = value,
                        Operator =  operato
                    }
                }
            };
        }
    }
}

using BlGrid.Api.Infrastructure;
using BlGrid.Api.Infrastructure.QueryHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading.Tasks;
using TestSupport.EfHelpers;

namespace BlGrid.Api.Tests
{
    [TestClass]
    public class BlGridRepositorySearchTextualTests
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
        public async Task Search_Textual_Equals()
        {
            await TestSearch("Adrian", FilterOperator.Equals, 1);
        }

        [TestMethod]
        public async Task Search_Textual_NoResult()
        {
            await TestSearch("Socio", FilterOperator.Equals, 0);
        }

        [TestMethod]
        public async Task Search_Textual_NotEquals()
        {
            await TestSearch("Adrian", FilterOperator.NotEquals, 3);
        }

        [TestMethod]
        public async Task Search_Textual_Contains()
        {
            await TestSearch("aco", FilterOperator.Contains, 2);
        }

        [TestMethod]
        public async Task Search_Textual_Contains_Additional_Or()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "String",
                        Type =  CellDataType.Text,
                        Value = "great",
                        Operator =   FilterOperator.Contains,
                        AdditionalOperator = FilterOperator.Contains,
                        AdditionalValue = "Paco",
                        Condition= FilterCondition.Or
                    }
                }
            };
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(3);
        }


        [TestMethod]
        public async Task Search_Textual_Contains_Additional_And()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "String",
                        Type =  CellDataType.Text,
                        Value = "great",
                        Operator =   FilterOperator.Contains,
                        AdditionalOperator = FilterOperator.Contains,
                        AdditionalValue = "Paco",
                        Condition= FilterCondition.And
                    }
                }
            };
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(1);
        }

        [TestMethod]
        public async Task Search_Textual_Contains_Additional_None()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "String",
                        Type =  CellDataType.Text,
                        Value = "great",
                        Operator =   FilterOperator.Contains,
                        AdditionalOperator = FilterOperator.Contains,
                        AdditionalValue = "Paco",
                        Condition= FilterCondition.None
                    }
                }
            };
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(2);
        }


        [TestMethod]
        public async Task Search_Textual_Contains_NoResults()
        {
            await TestSearch("Socio", FilterOperator.NotContains, 4);
        }

        [TestMethod]
        public async Task Search_Textual_NotContains()
        {
            await TestSearch("aco", FilterOperator.NotContains, 2);
        }

        [TestMethod]
        public async Task Search_Textual_NotContains_NoResults()
        {
            await TestSearch("Socio", FilterOperator.Contains, 0);
        }

        [TestMethod]
        public async Task Search_Textual_StartsWith()
        {
            await TestSearch("Paco", FilterOperator.StartsWith, 2);
        }

        [TestMethod]
        public async Task Search_Textual_StartsWith_NoResults()
        {
            await TestSearch("Socio", FilterOperator.StartsWith, 0);
        }

        [TestMethod]
        public async Task Search_Textual_EndsWith()
        {
            await TestSearch("great", FilterOperator.EndsWith, 2);
        }

        [TestMethod]
        public async Task Search_Textual_EndsWith_NoResults()
        {
            await TestSearch("the", FilterOperator.EndsWith, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Textual_GreaterThan()
        {
            await TestSearch("1", FilterOperator.GreaterThan, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Textual_LessThan()
        {
            await TestSearch("1", FilterOperator.LessThan, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Textual_Range()
        {
            await TestSearch("1", FilterOperator.Range, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Textual_None()
        {
            await TestSearch("1", FilterOperator.None, 0);
        }


        private async Task TestSearch(string value, FilterOperator operato, int total) 
        {
            var searchModel = BuildSearchModel(value, operato);
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(total);
        }

        private SearchModel BuildSearchModel(string value, FilterOperator operato)
        {
            return new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "String",
                        Type =  CellDataType.Text,
                        Value = value,
                        Operator =  operato
                    }
                }
            };
        }
    }
}

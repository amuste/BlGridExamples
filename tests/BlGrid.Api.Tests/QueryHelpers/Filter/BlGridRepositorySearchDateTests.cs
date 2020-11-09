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
    public class BlGridRepositorySearchDateTests
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
        public async Task Search_Date_Equals()
        {
            await DateSearch("2020-01-01", FilterOperator.Equals,"Date", 1,"yyyy-MM-dd");
        }

        [TestMethod]
        public async Task Search_Date_Equals_Nullable()
        {
            await DateSearch("2020-01-01", FilterOperator.Equals, "DateNullable", 1, "yyyy-MM-dd");
        }

        [TestMethod]
        public async Task Search_Date_Equals_Nullable_WithNull()
        {
            await DateSearch(null, FilterOperator.Equals, "DateNullable", 3, "yyyy-MM-dd");
        }

        [TestMethod]
        public async Task Search_Date_Equals_OtherFormat()
        {
            await DateSearch("01:01:2020", FilterOperator.Equals, "Date", 1, "MM:dd:yyyy");
        }

        [TestMethod]
        public async Task Search_Date_Equals_Datetime()
        {
            await DateSearch("2018-01-01 01:02:03", FilterOperator.Equals, "Date", 1, "yyyy-MM-dd hh:mm:ss");
        }

        [TestMethod]
        public async Task Search_Date_NotEquals()
        {
            await DateSearch("01:01:2019", FilterOperator.NotEquals, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        public async Task Search_Date_GreaterThan()
        {
            await DateSearch("01:01:2018", FilterOperator.GreaterThan, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        public async Task Search_Date_LessThan()
        {
            await DateSearch("01:01:2019", FilterOperator.LessThan, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Date_None()
        {
            await DateSearch("01:01:2018", FilterOperator.None, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Date_Contains()
        {
            await DateSearch("01:01:2018", FilterOperator.Contains, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Date_NotContains()
        {
            await DateSearch("01:01:2018", FilterOperator.NotContains, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Date_EndsWith()
        {
            await DateSearch("01:01:2018", FilterOperator.EndsWith, "Date", 3, "MM:dd:yyyy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Date_StartsWith()
        {
            await DateSearch("01:01:2018", FilterOperator.StartsWith, "Date", 3, "MM:dd:yyyy");
        }
        [TestMethod]
        public async Task Search_Date_Range()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        DateFormat = "MM:dd:yyyy",
                        Column = "Date",
                        Type =  CellDataType.Date,
                        Value = "01:01:2017",
                        AdditionalValue = "01:01:2019",
                        Operator =   FilterOperator.Range
                    }
                }
            };



            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(3);
        }


        private async Task DateSearch(string value, FilterOperator operato, string column,  int total, string dateFormat)
        {
            var searchModel = BuildSearchModel(value, operato, column, dateFormat);
            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(total);
        }

        private SearchModel BuildSearchModel(string value, FilterOperator operato, string column, string dateFormat)
        {
            return new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        DateFormat = dateFormat,
                        Column = column,
                        Type =  CellDataType.Date,
                        Value = value,
                        Operator =  operato
                    }
                }
            };
        }
    }
}

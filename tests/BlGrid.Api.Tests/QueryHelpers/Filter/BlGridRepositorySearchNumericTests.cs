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
    public class BlGridRepositorySearchNumericTests
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
        public async Task Search_Numeric_Int_Equals_NoResult()
        {
            await NumericSearch("5", FilterOperator.Equals, "Int", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task Search_Numeric_NoParse()
        {
            await NumericSearch("asd", FilterOperator.Equals, "Int", 0);
        }

        [TestMethod]
        public async Task Search_Numeric_Short_Equals()
        {
            await NumericSearch(short.MinValue.ToString(), FilterOperator.Equals, "Short", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_UShort_Equals()
        {
            await NumericSearch(ushort.MaxValue.ToString(), FilterOperator.Equals, "UShort", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_Equals()
        {
            await NumericSearch("30", FilterOperator.Equals, "Int", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_Equals_Additional_And()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "Int",
                        Type =  CellDataType.Number,
                        Value = "20",
                        Operator =   FilterOperator.GreaterThan,
                        AdditionalValue = "40",
                        AdditionalOperator = FilterOperator.LessThan,
                        Condition = FilterCondition.And
                    }
                }
            };

            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(1);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_Equals_Additional_Or()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "Int",
                        Type =  CellDataType.Number,
                        Value = "20",
                        Operator =   FilterOperator.GreaterThan,
                        AdditionalValue = "10",
                        AdditionalOperator = FilterOperator.Equals,
                        Condition = FilterCondition.Or
                    }
                }
            };

            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(3);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_Equals_Additional_None()
        {
            var searchModel = new SearchModel
            {
                AdvancedFilterModels = new System.Collections.Generic.List<AdvancedFilterModel>(){
                    new AdvancedFilterModel{
                        Column = "Int",
                        Type =  CellDataType.Number,
                        Value = "20",
                        Operator =   FilterOperator.GreaterThan,
                        AdditionalValue = "10",
                        AdditionalOperator = FilterOperator.Equals,
                           Condition = FilterCondition.None
                    }
                }
            };

            var entities = await _repository.Search(searchModel);
            entities.Count.ShouldBe(2);
        }

        [TestMethod]
        public async Task Search_Numeric_UInt_Equals()
        {
            await NumericSearch(uint.MaxValue.ToString(), FilterOperator.Equals, "UInt", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_Long_Equals()
        {
            await NumericSearch(long.MinValue.ToString(), FilterOperator.Equals, "Long", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_ULong_Equals()
        {
            await NumericSearch(ulong.MaxValue.ToString(), FilterOperator.Equals, "ULong", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableInt_Equals()
        {
            await NumericSearch(int.MaxValue.ToString(), FilterOperator.Equals, "IntNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableInt_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "IntNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableUInt_Equals()
        {
            await NumericSearch(uint.MaxValue.ToString(), FilterOperator.Equals, "UIntNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableUInt_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "UIntNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableLong_Equals()
        {
            await NumericSearch(long.MaxValue.ToString(), FilterOperator.Equals, "LongNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableLong_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "LongNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_Nullable_ULong_Equals()
        {
            await NumericSearch(ulong.MaxValue.ToString(), FilterOperator.Equals, "ULongNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableULong_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "ULongNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableShort_Equals()
        {
            await NumericSearch(short.MaxValue.ToString(), FilterOperator.Equals, "ShortNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableShort_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "ShortNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableUShort_Equals()
        {
            await NumericSearch(ushort.MaxValue.ToString(), FilterOperator.Equals, "UShortNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableUShort_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "UShortNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_Decimal_Equals()
        {
            await NumericSearch(decimal.MaxValue.ToString(), FilterOperator.Equals, "Decimal", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableDecimal_Equals()
        {
            await NumericSearch(decimal.MaxValue.ToString(), FilterOperator.Equals, "DecimalNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableDecimal_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "DecimalNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_Float_Equals()
        {
            await NumericSearch(float.MaxValue.ToString(), FilterOperator.Equals, "Float", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_Float_Equals_StandardWithDot()
        {
            await NumericSearch("3.25", FilterOperator.Equals, "Float", 1);
        }


        [TestMethod]
        public async Task Search_Numeric_Float_Equals_StandardWithComma()
        {
            await NumericSearch("3,25", FilterOperator.Equals, "Float", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableFloat_Equals()
        {
            await NumericSearch(float.MaxValue.ToString(), FilterOperator.Equals, "FloatNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableFloat_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "FloatNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_Double_Equals()
        {
            await NumericSearch(double.MaxValue.ToString(), FilterOperator.Equals, "Double", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_Double_Equals_StandardWithDot()
        {
            await NumericSearch("5.25", FilterOperator.Equals, "Double", 1);
        }


        [TestMethod]
        public async Task Search_Numeric_Double_Equals_StandardWithComma()
        {
            await NumericSearch("5,25", FilterOperator.Equals, "Double", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableDouble_Equals()
        {
            await NumericSearch(double.MaxValue.ToString(), FilterOperator.Equals, "DoubleNullable", 1);
        }

        [TestMethod]
        public async Task Search_Numeric_NullableDouble_Equals_WithNull()
        {
            await NumericSearch(null, FilterOperator.Equals, "DoubleNullable", 3);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_NotEquals()
        {
            await NumericSearch("30", FilterOperator.NotEquals, "Int", 3);
        }


        [TestMethod]
        public async Task Search_Numeric_Int_GreaterThan()
        {
            await NumericSearch("20", FilterOperator.GreaterThan, "Int", 2);
        }

        [TestMethod]
        public async Task Search_Numeric_Int_LessThan()
        {
            await NumericSearch("20", FilterOperator.LessThan, "Int", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_Contains()
        {
            await NumericSearch("2", FilterOperator.Contains, "Int", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_NotContains()
        {
            await NumericSearch("2", FilterOperator.NotContains, "Int", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_EndsWith()
        {
            await NumericSearch("2", FilterOperator.EndsWith, "Int", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_None()
        {
            await NumericSearch("2", FilterOperator.None, "Int", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_Range()
        {
            await NumericSearch("2", FilterOperator.Range, "Int", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Search_Numeric_Int_StartsWith()
        {
            await NumericSearch("2", FilterOperator.StartsWith, "Int", 3);
        }

        private async Task NumericSearch(string value, FilterOperator operato, string column,  int total)
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
                        Type =  CellDataType.Number,
                        Value = value,
                        Operator =  operato
                    }
                }
            };
        }
    }
}

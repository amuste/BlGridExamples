using System.Linq.Expressions;

namespace BlGrid.Api.Infrastructure.QueryHelpers.FilterHelpers
{
    public class FilterPredicate
    {
        public Expression Predicate { get; set; }

        public FilterCondition OperatorType { get; set; }
    }
}

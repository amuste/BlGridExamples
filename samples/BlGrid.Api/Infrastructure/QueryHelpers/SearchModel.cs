using System.Collections.Generic;

namespace BlGrid.Api.Infrastructure.QueryHelpers
{
    public class SearchModel
    {
        public PaginationModel PaginationModel { get; set; }

        public List<AdvancedFilterModel> AdvancedFilterModels { get; set; }

        public List<FilterModel> FilterModels { get; set; }

        public SortModel SortModel { get; set; }
    }
}

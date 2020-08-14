namespace BlGrid.Api.Infrastructure.QueryHelpers
{
    public class PaginationModel
    {
        public int? PageSize { get; set; }

        public int? CurrentPage { get; set; }

        public int? ItemsCount { get; set; }
    }
}

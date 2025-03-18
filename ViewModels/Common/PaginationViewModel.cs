namespace RealEStateProject.ViewModels.Common
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
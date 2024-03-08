namespace MedicineMarketPlace.Admin.Application
{
    public class BaseQueryDto
    {
        public string SearchText { get; set; }

        public string SortBy { get; set; }

        public int SortDir { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}

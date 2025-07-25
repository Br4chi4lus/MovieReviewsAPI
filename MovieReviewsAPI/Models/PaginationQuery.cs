namespace MovieReviewsAPI.Models
{
    public class PaginationQuery
    {
        public string? SearchPhase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

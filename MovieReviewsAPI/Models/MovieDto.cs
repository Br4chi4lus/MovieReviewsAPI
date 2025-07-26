namespace MovieReviewsAPI.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfPremiere { get; set; }
        public string Director { get; set; }
        public double AverageRating { get; set; }
    }
}

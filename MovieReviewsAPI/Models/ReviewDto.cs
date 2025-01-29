namespace MovieReviewsAPI.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastModification { get; set; }
    }
}

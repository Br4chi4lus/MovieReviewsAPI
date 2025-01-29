namespace MovieReviewsAPI.Models
{
    public class UpdateReviewContentDto
    {
        /// <summary>
        /// Updated content of the review
        /// </summary>
        /// <example>This movie is very boring...</example>
        public string Content { get; set; }
    }
}

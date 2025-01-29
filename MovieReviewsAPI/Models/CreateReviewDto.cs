namespace MovieReviewsAPI.Models
{
    public class CreateReviewDto
    {
        /// <summary>
        /// Content of the review, what the reviewer thinks about the movie
        /// </summary>
        /// <example>This movie is very interesting...</example>
        public string Content { get; set; }
        /// <summary>
        /// Rating of the movie from - integer 1 to 10
        /// </summary>
        /// <example>7</example>
        public int Rating { get; set; }
    }
}

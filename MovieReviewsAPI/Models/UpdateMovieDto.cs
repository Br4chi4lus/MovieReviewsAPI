namespace MovieReviewsAPI.Models
{
    public class UpdateMovieDto
    {
        /// <summary>
        /// Updated date of premiere
        /// </summary>
        /// <example>1986-05-12</example>
        public DateTime? DateOfPremiere { get; set; }
        /// <summary>
        /// Updated Description
        /// </summary>
        /// <example>Something about the movie etc</example>
        public string? Description { get; set; }
    }
}

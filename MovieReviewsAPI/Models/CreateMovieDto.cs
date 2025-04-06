using System.ComponentModel.DataAnnotations;

namespace MovieReviewsAPI.Models
{
    public class CreateMovieDto
    {
        /// <summary>
        /// Title of the movie
        /// </summary>
        /// <example>Top Gun</example>
        /// 
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }
        /// <summary>
        /// What is the movie about
        /// </summary>
        /// <example>Some description</example>
        public string? Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>Toni</example>
        [Required]
        public string DirectorFirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>Scott</example>
        [Required]
        public string DirectorLastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>1986-05-12</example>
        public DateTime? DateOfPremiere { get; set; }
    }
}

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace MovieReviewsAPI.Models
{
    public class LoginUserDto
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        /// <example>test@test.com</example>
        [SwaggerSchema(Description = "Email used to register")]
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <example>password123</example>
        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public string Password { get; set; }
    }
}

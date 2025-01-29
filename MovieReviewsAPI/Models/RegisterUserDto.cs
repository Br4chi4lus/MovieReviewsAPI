using System.ComponentModel.DataAnnotations;

namespace MovieReviewsAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    ///
    public class RegisterUserDto
    {
        /// <summary>
        /// Email of new User
        /// </summary>
        /// <example>test@test.com</example>
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; }
        /// <example>password123</example>
        [Required]
        [MinLength(8)]
        [MaxLength(64)]       
        public string Password { get; set; }
        /// <summary>
        /// Must be equal to Password
        /// </summary>
        /// <example>password123</example>
        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public string PasswordConfirm { get; set; }
        /// <summary>
        /// UserName that will be displayed in reviews
        /// </summary>
        /// <example>SomeUser</example>
        [Required]
        [MaxLength(32)]
        public string UserName { get; set; }
        /// <example>John</example>
        [Required]
        public string FirstName { get; set; }   
        /// <example>Doe</example>
        public string? LastName { get; set; }
        /// <example>1999-01-10</example>
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}

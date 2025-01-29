using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewsAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreationDate { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}

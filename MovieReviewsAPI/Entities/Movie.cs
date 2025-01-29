using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewsAPI.Entities
{
    public class Movie
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DirectorId { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfPremiere { get; set; }
        public virtual Director Director { get; set; }
        public virtual List<Review> Reviews { get; set; }        
    }
}

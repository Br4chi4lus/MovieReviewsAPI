namespace MovieReviewsAPI.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User Author { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public int MovieId { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastModification { get; set; }
        public virtual Movie Movie { get; set; }
    }
}

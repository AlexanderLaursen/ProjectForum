namespace WebApi.Models
{
    public class CommentLike
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser User { get; set; }
        public Comment Comment { get; set; }
    }
}

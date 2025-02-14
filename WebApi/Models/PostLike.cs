namespace WebApi.Models
{
    public class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; }
        public AppUser User { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class PostHistory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }

        public IdentityUser User { get; set; }
        public Post Post { get; set; }
    }
}

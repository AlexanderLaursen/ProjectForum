using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class CommentHistory
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }

        public AppUser User { get; set; }
        public Comment Comment { get; set; }
    }
}
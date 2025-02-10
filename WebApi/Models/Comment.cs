using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public bool Active { get; set; }
        public bool Edited { get; set; }
        public DateTime EditedAt { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }


        public AppUser User { get; set; }
        public Post Post { get; set; }
        public List<CommentHistory> CommentHistory { get; set; }
    }
}

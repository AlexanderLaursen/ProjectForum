using Microsoft.AspNetCore.Identity;

namespace MVC.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public bool Active { get; set; }
        public bool Edited { get; set; }
        public DateTime EditedAt { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }


        public AppUser User { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<PostHistory> PostHistory { get; set; }
    }
}

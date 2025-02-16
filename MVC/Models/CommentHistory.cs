using Common.Dto.Comment;
using Common.Dto.CommentHistory;

namespace MVC.Models
{
    public class CommentHistory
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
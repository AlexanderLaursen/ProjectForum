using Common.Models;

namespace MVC.Models
{
    public class CommentHistories
    {
        public List<CommentHistory> CommentHistory { get; set; }
        public PageInfo PageInfo { get; set; }
        public Comment Comment { get; set; }
    }
}

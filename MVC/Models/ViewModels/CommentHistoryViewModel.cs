namespace MVC.Models.ViewModels
{
    public class CommentHistoryViewModel
    {
        public Comment Comment { get; set; }
        public List<CommentHistory> CommentHistory { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
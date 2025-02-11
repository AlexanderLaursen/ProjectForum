namespace MVC.Models.ViewModels
{
    public class UpdateCommentViewModel
    {
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; }
    }
}
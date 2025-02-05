namespace MVC.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

namespace MVC.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public PageInfo PageInfo { get; set; }
    }
}

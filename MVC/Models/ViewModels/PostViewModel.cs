namespace MVC.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; } = new Post();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}

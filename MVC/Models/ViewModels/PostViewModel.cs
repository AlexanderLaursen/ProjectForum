namespace MVC.Models.ViewModels
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public Post Post { get; set; } = new Post();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}

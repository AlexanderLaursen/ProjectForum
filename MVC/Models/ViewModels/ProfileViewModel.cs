namespace MVC.Models.ViewModels
{
    public class ProfileViewModel
    {
        public AppUser User { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public PageInfo PostPageInfo { get; set; }
        public PageInfo CommentPageInfo { get; set; }
    }
}
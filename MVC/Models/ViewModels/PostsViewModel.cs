using Common.Enums;

namespace MVC.Models.ViewModels
{
    public class PostsViewModel
    {
        public Category Category { get; set; } = new Category();
        public List<Post> Posts { get; set; } = new List<Post>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
        public SortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}

namespace MVC.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

using Common.Enums;
using Common.Models;

namespace MVC.Models.ViewModels
{
    public class PostsViewModel
    {
        public Category? Category { get; set; } = new();
        public List<Post> Posts { get; set; } = [];
        public PageInfo PageInfo { get; set; } = new();
        public SortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}

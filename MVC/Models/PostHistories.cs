using Common.Models;

namespace MVC.Models
{
    public class PostHistories
    {
        public List<PostHistory> PostHistory { get; set; } = new List<PostHistory>();
        public Post Post { get; set; } = new Post();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
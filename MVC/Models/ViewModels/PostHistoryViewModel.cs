namespace MVC.Models.ViewModels
{
    public class PostHistoryViewModel
    {
        public Post Post { get; set; }
        public List<PostHistory> PostHistory { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
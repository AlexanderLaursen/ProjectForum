namespace MVC.Models
{
    public class PostHistory
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
namespace MVC.Models.ViewModels
{
    public class UpdatePostViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }

        public Post? Post { get; set; }
    }
}

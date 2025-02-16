namespace Common.Dto.Post
{
    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }
    }
}

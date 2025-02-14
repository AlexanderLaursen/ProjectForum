namespace MVC.Models.Dto
{
    public class PostDetailsDto
    {
        public Post PostDto { get; set; }
        public List<Comment> Comments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

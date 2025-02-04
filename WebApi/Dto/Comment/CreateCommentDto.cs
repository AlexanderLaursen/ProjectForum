namespace WebApi.Dto.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
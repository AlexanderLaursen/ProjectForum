namespace WebApi.Dto.Comment
{
    public class CommentHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
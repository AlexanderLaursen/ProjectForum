namespace WebApi.Dto.Comment
{
    public class CreateCommentHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
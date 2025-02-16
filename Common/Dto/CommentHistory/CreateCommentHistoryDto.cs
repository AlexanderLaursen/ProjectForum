namespace Common.Dto.CommentHistory
{
    public class CreateCommentHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
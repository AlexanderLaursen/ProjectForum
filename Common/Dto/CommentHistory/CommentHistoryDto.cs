namespace Common.Dto.CommentHistory
{
    public class CommentHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
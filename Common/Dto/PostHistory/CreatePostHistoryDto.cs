namespace Common.Dto.PostHistory
{
    public class CreatePostHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace WebApi.Dto.PostHistory
{
    public class PostHistoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }
    }
}
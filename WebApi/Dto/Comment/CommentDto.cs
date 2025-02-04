using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }

        public string UserId { get; set; }
        public UserDto User { get; set; }
    }
}

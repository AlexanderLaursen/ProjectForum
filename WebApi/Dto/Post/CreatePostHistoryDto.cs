using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Dto.Post
{
    public class CreatePostHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
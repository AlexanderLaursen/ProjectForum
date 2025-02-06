using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Dto.Post
{
    public class PostHistoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
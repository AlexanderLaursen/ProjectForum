using Common.Dto.Comment;
using Common.Dto.Post;
using Common.Models;
using MVC.Models;

namespace MVC.Services
{
    public class PostDetails
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
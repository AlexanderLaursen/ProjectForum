using WebApi.Dto.Comment;
using WebApi.Dto.Post;
using WebApi.Models;

namespace WebApi.Dto
{
    public class PostDetailsDto
    {
        public PostDto PostDto { get; set; }
        public List<CommentDto> Comments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

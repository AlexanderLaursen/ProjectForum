using Common.Dto.Comment;
using Common.Models;

namespace Common.Dto.Post
{
    public class PostDetailsDto
    {
        public PostDto PostDto { get; set; }
        public List<CommentDto> CommentsDto { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

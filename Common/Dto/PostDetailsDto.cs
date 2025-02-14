using Common.Models;

namespace Common.Dto
{
    public class PostDetailsDto
    {
        public PostDto PostDto { get; set; }
        public List<CommentDto> Comments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
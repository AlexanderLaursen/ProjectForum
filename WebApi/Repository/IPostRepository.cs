using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IPostRepository
    {
        public OperationResult<Post> CreatePost(CreatePostDto createPostDto, string userId);
        public OperationResult<PostDto> GetPostById(int postId);
    }
}

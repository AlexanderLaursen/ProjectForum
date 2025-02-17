using Common.Dto.Post;
using Common.Models;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Result<PostDto>> GetPostAsync(int postId)
        {
            return await _postRepository.GetPostAsync(postId);
        }
    }
}

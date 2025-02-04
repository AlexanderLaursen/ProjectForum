using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IPostRepository
    {
        public Task<OperationResult> GetPostByIdAsync(int postId);
        public Task<OperationResult> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo);
        public Task<OperationResult> CreatePostAsync(CreatePostDto createPostDto, string userId);
    }
}

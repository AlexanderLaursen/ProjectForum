using Common.Enums;
using Common.Dto.Post;
using WebApi.Models;
using WebApi.Models.Ope;
using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IPostRepository
    {
        public Task<OperationResultNew> GetPostDetailsAsync(int postId, string userId, PageInfo pageInfo);
        public Task<OperationResult> GetPostByIdAsync(int postId, PageInfo pageInfo);
        public Task<OperationResult> GetPostsByUsernameAsync(string userId, PageInfo pageInfo);
        public Task<OperationResult> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo, SortBy sortBy, SortDirection sortDirection);
        public Task<OperationResult> GetPostHistoryByPostId(int postId, PageInfo pageInfo);
        public Task<OperationResult> CreatePostAsync(string userId, CreatePostDto createPostDto);
        public Task<OperationResult> UpdatePostAsync(string userId, UpdatePostDto updatePostDto);
        public Task<OperationResult> DeletePostAsync(int postId, string userId);
    }
}

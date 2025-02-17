using Common.Dto.Post;
using Common.Enums;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Result<PostDto>> GetPostAsync(int postId, string? userId);
        public Task<Result<PagedPostsDto>> GetPostsByCategoryAsync(int categoryId, PageInfo pageInfo, SortBy sortBy, SortDirection sortDirection);
        public Task<Result<PagedPostsDto>> GetPostsByUsernameAsync(string username, PageInfo pageInfo);
        public Task<Result<PostDto>> CreatePostAsync(string userId, CreatePostDto createPostDto);
        public Task<Result<PostDto>> UpdatePostAsync(int postId, string userId, UpdatePostDto updatePostDto);
        public Task<Result<PostDto>> DeletePostAsync(int postId, string userId);
    }
}

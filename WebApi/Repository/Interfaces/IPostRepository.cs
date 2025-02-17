using Common.Enums;
using Common.Dto.Post;
using WebApi.Models;
using WebApi.Models.Ope;
using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IPostRepository
    {
        public Task<Result<PostDto>> GetPostAsync(int postId, string? userId = null);
        public Task<Result<Post>> GetFullPostAsync(int postId);
        public Task<Result<PagedPostsDto>> GetPostsByCategoryAsync(int categoryId, PageInfo pageInfo, SortBy sortBy, SortDirection sortDirection);
        public Task<Result<PagedPostsDto>> GetPostsByUserIdAsync(string userId, PageInfo pageInfo);
        public Task<Result<PostDto>> CreatePostAsync(string userId, CreatePostDto createPostDto);
        public Task<Result<PostDto>> UpdatePostAsync(Post post, UpdatePostDto updatePostDto);
        public Task<Result<PostDto>> DeletePostAsync(Post post);
    }
}

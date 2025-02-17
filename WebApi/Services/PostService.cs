using Common.Dto.Post;
using Common.Dto.Post;
using Common.Dto.User;
using Common.Enums;
using Common.Models;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

      
        public async Task<Result<PostDto>> GetPostAsync(int postId, string? userId)
        {
            return await _postRepository.GetPostAsync(postId, userId);
        }

        public async Task<Result<PagedPostsDto>> GetPostsByCategoryAsync(int categoryId, PageInfo pageInfo, SortBy sortBy, SortDirection sortDirection)
        {
            return await _postRepository.GetPostsByCategoryAsync(categoryId, pageInfo, sortBy, sortDirection);
        }

        public async Task<Result<PagedPostsDto>> GetPostsByUsernameAsync(string username, PageInfo pageInfo)
        {
            Result<UserDto> userDto = await _userRepository.GetUserAsync(username);
            
            if (!userDto.IsSuccess || userDto.Value == null)
            {
                return Result<PagedPostsDto>.NotFound();
            }

            return await _postRepository.GetPostsByUserIdAsync(userDto.Value.Id, pageInfo);
        }

        public async Task<Result<PostDto>> CreatePostAsync(string userId, CreatePostDto createPostDto)
        {
            return await _postRepository.CreatePostAsync(userId, createPostDto);
        }
        public async Task<Result<PostDto>> UpdatePostAsync(int postId, string userId, UpdatePostDto updatePostDto)
        {
            Result<Post> result = await _postRepository.GetFullPostAsync(postId);

            if (!result.IsSuccess)
            {
                return Result<PostDto>.ConvertDtoError<Post, PostDto>(result);
            }

            if (result.Value == null)
            {
                return Result<PostDto>.NotFound();
            }

            if (result.Value.UserId != userId)
            {
                return Result<PostDto>.Unauthorized();
            }

            return await _postRepository.UpdatePostAsync(result.Value, updatePostDto);
        }

        public async Task<Result<PostDto>> DeletePostAsync(int postId, string userId)
        {
            Result<Post> result = await _postRepository.GetFullPostAsync(postId);

            if (!result.IsSuccess)
            {
                return Result<PostDto>.ConvertDtoError<Post, PostDto>(result);
            }

            if (result.Value == null)
            {
                return Result<PostDto>.NotFound();
            }

            if (result.Value.UserId != userId)
            {
                return Result<PostDto>.Unauthorized();
            }

            return await _postRepository.DeletePostAsync(result.Value);
        }
    }
}

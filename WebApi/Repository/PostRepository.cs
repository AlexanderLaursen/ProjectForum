using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public OperationResult<PostDto> GetPostById(int postId)
        {
            try
            {
                var post = _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .FirstOrDefault(p => p.Id == postId);

                if (post == null)
                {
                    return new OperationResult<PostDto>
                    {
                        Success = false,
                        ErrorMessage = "Post not found"
                    };
                }

                PostDto postDto = post.Adapt<PostDto>();

                return new OperationResult<PostDto>
                {
                    Success = true,
                    Data = postDto
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<PostDto>
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult<List<Post>>> GetPostsByCategoryId (int categoryId)
        {
            return new OperationResult<List<Post>>
            {
                Success = true,
                Data = await _context.Posts.Where(p => p.CategoryId == categoryId).ToListAsync()
            };
        }

        public OperationResult<Post> CreatePost(CreatePostDto createPostDto, string userId)
        {
            try
            {
                Post post = createPostDto.Adapt<Post>();
                post.UserId = userId;
                _context.Posts.Add(post);
                _context.SaveChanges();

                return new OperationResult<Post>
                {
                    Success = true,
                    Data = post
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<Post>
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }
    }
}

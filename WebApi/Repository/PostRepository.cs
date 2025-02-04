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


        // TODO Implement input validation
        public async Task<OperationResult> GetPostByIdAsync(int postId)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Post not found."
                    };
                }

                PostDto postDto = post.Adapt<PostDto>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        ["content"] = postDto
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        // Spørgsmål omkring PaginatedOperationResult
        public async Task<OperationResult> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo)
        {
            if (categoryId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid category id."
                };
            }

            try
            {
                IQueryable<Post> query = _context.Posts
                    .Include(p => p.User)
                    .Where(p => p.CategoryId == categoryId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                int totalItems = query.Count();

                if (totalItems == 0)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "No posts found for the given category."
                    };
                }

                List<Post> posts = await query.ToListAsync();
                List<PostDto> postsDto = posts.Adapt<List<PostDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        ["content"] = postsDto,
                        ["pagination"] = new PageInfo
                        {
                            CurrentPage = pageInfo.CurrentPage,
                            PageSize = pageInfo.PageSize,
                            TotalItems = totalItems
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }


        // TODO: Implement validation for CreatePostDto
        public async Task<OperationResult> CreatePostAsync(CreatePostDto createPostDto, string userId)
        {
            try
            {
                Post post = createPostDto.Adapt<Post>();
                post.UserId = userId;
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                PostDto postDto = post.Adapt<PostDto>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        ["content"] = postDto
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }
    }
}

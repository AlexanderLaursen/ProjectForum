using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration.UserSecrets;
using WebApi.Data;
using WebApi.Dto.Post;
using WebApi.Models;

namespace WebApi.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly ICommonRepository _commonRepository;

        public PostRepository(DataContext context, ICommonRepository commonRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
        }

        public async Task<OperationResult> GetPostByIdAsync(int postId)
        {
            if (postId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid post id."
                };
            }

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
                List<PostDto> postList = [postDto];

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postList }
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
                    .Where(p => p.CategoryId == categoryId);

                int totalItems = query.Count();

                var queryWithPagination = query.Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                List<Post> posts = await queryWithPagination.ToListAsync();
                List<PostDto> postsDto = posts.Adapt<List<PostDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postsDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
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

        public async Task<OperationResult> GetPostsByUsernameAsync(string username, PageInfo pageInfo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            string? userId = await _commonRepository.GetUserIdByUsernameAsync(username);

            if (string.IsNullOrEmpty(userId))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            try
            {
                IQueryable<Post> query = _context.Posts
                    .Include(p => p.User)
                    .Where(p => p.UserId == userId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                int totalItems = query.Count();

                List<Post> posts = await query.ToListAsync();
                List<PostDto> postsDto = posts.Adapt<List<PostDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postsDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
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

        public async Task<OperationResult> GetPostHistoryByPostId(int postId, PageInfo pageInfo)
        {
            if (postId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid post id."
                };
            }

            try
            {
                IQueryable<PostHistory> query = _context.PostHistory
                    .Where(ph => ph.PostId == postId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                int totalItems = query.Count();

                List<PostHistory> postHistories = await query.ToListAsync();
                List<PostHistoryDto> postHistoriesDto = postHistories.Adapt<List<PostHistoryDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postHistoriesDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
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

        // TODO: Implement input validation for createPostDto
        public async Task<OperationResult> CreatePostAsync(string userId, CreatePostDto createPostDto)
        {
            try
            {
                Post post = createPostDto.Adapt<Post>();
                post.UserId = userId;
                post.CreatedAt = DateTime.Now;
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                PostDto postDto = post.Adapt<PostDto>();
                List<PostDto> postList = [postDto];

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postList }
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

        // TODO: Implement input validation for createPostDto
        public async Task<OperationResult> UpdatePostAsync(string userId, UpdatePostDto updatePostDto)
        {
            if (updatePostDto.PostId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid post id."
                };
            }

            if (userId == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == updatePostDto.PostId);

            if (post == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Post not found."
                };
            }

            if (post.UserId != userId)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized user."
                };
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
                try
                {
                    PostHistory postHistory = post.Adapt<PostHistory>();
                    postHistory.Id = new int();
                    postHistory.PostId = post.Id;
                    postHistory.CreatedAt = post.EditedAt == DateTime.MinValue ? post.CreatedAt : post.EditedAt;
                    _context.PostHistory.Add(postHistory);
                    await _context.SaveChangesAsync();

                    int postHistoryId = postHistory.Id;

                    post.PostHistory.Add(postHistory);
                    post.Edited = true;
                    post.EditedAt = DateTime.Now;
                    post.Content = updatePostDto.Content;
                    _context.Posts.Update(post);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    PostDto postDto = post.Adapt<PostDto>();
                    List<PostDto> postList = [postDto];

                    return new OperationResult
                    {
                        Success = true,
                        Data = new Dictionary<string, object>
                        {
                            { "content", postList }
                        }
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                    };
                }
        }

        public async Task<OperationResult> DeletePostAsync(int postId, string userId)
        {
            if (postId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid post id."
                };
            }

            if (userId == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Post not found."
                };
            }

            if (post.UserId != userId)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized user."
                };
            }

            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return new OperationResult
                {
                    Success = true
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

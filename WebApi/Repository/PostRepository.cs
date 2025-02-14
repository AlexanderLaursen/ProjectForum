using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration.UserSecrets;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.InteropServices.ObjectiveC;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Dto.Comment;
using WebApi.Dto.Post;
using WebApi.Dto.PostHistory;
using WebApi.Models;
using Common.Enums;
using System.Diagnostics.Eventing.Reader;

namespace WebApi.Repository
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        private readonly ICommonRepository _commonRepository;

        public PostRepository(DataContext context, ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }

        public async Task<OperationResultNew> GetPostDetailsAsync(int postId, string userId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.Posts
                    .Where(p => p.Id == postId)
                    .Select(p => new
                    {
                        Post = p,
                        LikedByUser = p.PostLikes.Any(pl => pl.UserId == userId),
                        Likes = p.PostLikes.Count(),
                        p.User,

                        CommentsQuery = p.Comments.OrderBy(c => c.CreatedAt)
                            .Select(c => new
                            {
                                Comment = c,
                                LikedByUser = c.CommentLikes.Any(cl => cl.UserId == userId),
                                Likes = c.CommentLikes.Count(),
                                c.User
                            })
                    });

                var result = await query.Select(q => new
                {
                    q.Post,
                    q.LikedByUser,
                    q.Likes,
                    q.User,

                    TotalComments = q.CommentsQuery.Count(),

                    Comments = q.CommentsQuery
                        .Skip(pageInfo.Skip)
                        .Take(pageInfo.PageSize)
                        .ToList()
                })
                .SingleOrDefaultAsync();

                if (result == null)
                {
                    return OperationResultNew.Fail("Not found.");
                }

                PostDto postDto = result.Post.Adapt<PostDto>();
                postDto.Likes = result.Likes;
                postDto.LikedByUser = result.LikedByUser;

                List<CommentDto> comments = [];
                foreach (var comment in result.Comments)
                {
                    CommentDto commentDto = comment.Comment.Adapt<CommentDto>();
                    commentDto.LikedByUser = comment.LikedByUser;
                    commentDto.Likes = comment.Likes;
                    commentDto.User = comment.User.Adapt<ShortUserDto>();
                    comments.Add(commentDto);
                }

                PageInfo newPageInfo = new PageInfo(pageInfo.CurrentPage, pageInfo.PageSize, result.TotalComments);

                PostDetailsDto postDetailsDto = new PostDetailsDto
                {
                    PostDto = postDto,
                    Comments = comments,
                    PageInfo = newPageInfo
                };

                return OperationResultNew.IsSuccess(postDetailsDto);
            }
            catch (Exception)
            {
                return OperationResultNew.Fail();
            }
        }

        public async Task<OperationResult> GetPostByIdAsync(int postId, PageInfo pageInfo)
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
                // TODO: Error handling (check for null)
                var post = await _context.Posts
                    .Where(p => p.Id == postId)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync();

                var postLikes = await _context.PostLikes
                    .Where(pl => pl.PostId == postId)
                    .CountAsync();

                int totalComments = await _context.Comments
                    .Where(c => c.PostId == postId)
                    .CountAsync();

                var comments = await _context.Comments
                    .Where(c => c.PostId == postId)
                    .Include(c => c.User)
                    .Include(c => c.CommentLikes)
                    .OrderBy(c => c.Id)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                foreach (var comment in comments)
                {
                    comment.Likes = comment.CommentLikes.Count;
                }

                PostDto postDto = post.Adapt<PostDto>();
                postDto.Comments = comments.Adapt<List<CommentDto>>();
                List<PostDto> postList = [postDto];

                await IncrementViewCountAsync(postId);

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postList },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalComments,
                            }
                        }
                    },
                    InternalData = post
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

        public async Task<OperationResult> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo, SortBy sortBy = SortBy.Date, SortDirection sortDirection = SortDirection.Desc)
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
                IQueryable<Post> postsQuery = _context.Posts.AsNoTracking();

                if (sortBy == SortBy.Title)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderBy(p => p.Title)
                        : _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderByDescending(p => p.Title);
                }
                else if (sortBy == SortBy.Views)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderBy(p => p.ViewCount)
                        : _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderByDescending(p => p.ViewCount);
                }
                else if (sortBy == SortBy.Likes)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderBy(p => p.PostLikes.Count)
                        : _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderByDescending(p => p.PostLikes.Count);
                }
                else
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderBy(p => p.CreatedAt)
                        : _context.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .OrderByDescending(p => p.CreatedAt);
                }

                int totalItems = postsQuery.Count();

                var postsDtoList = await postsQuery.Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    Likes = post.PostLikes.Count,
                    ViewCount = post.ViewCount,
                    Edited = post.Edited,
                    CategoryId = post.CategoryId,
                    User = post.User.Adapt<ShortUserDto>()
                }).ToListAsync();

                //IQueryable<Post> query = _context.Posts
                //    .Include(p => p.User)
                //    .Where(p => p.CategoryId == categoryId);

                //int totalItems = query.Count();

                //var queryWithPagination = query
                //    .Skip(pageInfo.Skip)
                //    .Take(pageInfo.PageSize);

                //List<Post> posts = await queryWithPagination.ToListAsync();
                //List<PostDto> postsDto = posts.Adapt<List<PostDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", postsDtoList },
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
                var posts = await _context.Posts
                    .Include(p => p.User)
                    .Where(p => p.UserId == userId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (posts == null)
                {
                    return new OperationResult();
                }

                int totalItems = _context.Posts
                    .Where(p => p.UserId == userId)
                    .Count();

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

            Post? post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == updatePostDto.PostId);

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
                    PostHistory postHistory = new()
                    {
                        Title = post.Title,
                        Content = post.Content,
                        PostId = post.Id,
                        CategoryId = post.CategoryId,
                        UserId = post.UserId,
                        CreatedAt = post.EditedAt == DateTime.MinValue ? post.CreatedAt : post.EditedAt,
                        User = post.User,
                        Post = post,
                    };
                    _context.PostHistory.Add(postHistory);
                    await _context.SaveChangesAsync();

                    int postHistoryId = postHistory.Id;

                    post.PostHistory.Add(postHistory);
                    post.Edited = true;
                    post.EditedAt = DateTime.UtcNow;
                    post.Content = updatePostDto.Content;
                    post.Title = updatePostDto.Title;
                    post.CategoryId = updatePostDto.CategoryId;
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

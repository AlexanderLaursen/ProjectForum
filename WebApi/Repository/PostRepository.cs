using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Dto.Comment;
using Common.Dto.Post;
using Common.Dto.PostHistory;
using WebApi.Models;
using Common.Enums;
using Common.Models;
using Common.Dto.User;
using WebApi.Repository.Interfaces;
using WebApi.Models.Ope;

namespace WebApi.Repository
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(DataContext context, ILogger<PostRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Result<PostDto>> GetPostAsync(int postId, string? userId = null)
        {
            try
            {
                PostDto? postDto = await _context.Posts
                    .Where(r => r.Id == postId)
                    .Select(r => new PostDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Content = r.Content,
                        CreatedAt = r.CreatedAt,
                        Likes = r.PostLikes.Count,
                        Edited = r.Edited,
                        LikedByUser = r.PostLikes.Any(pl => pl.UserId == userId),
                        CategoryId = r.CategoryId,
                        User = r.User.Adapt<ShortUserDto>()
                    }).FirstOrDefaultAsync();

                if (postDto == null)
                {
                    return Result<PostDto>.NotFound();
                }

                return Result<PostDto>.Success(postDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting post from database.");
                return Result<PostDto>.Failure("Unkown error while fetching post.");
            }
        }

        public async Task<Result<Post>> GetFullPostAsync(int postId)
        {
            try
            {
                Post? post = await _context.Posts
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post == null)
                {
                    return Result<Post>.NotFound();
                }

                return Result<Post>.Success(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching post from database.");
                return Result<Post>.Failure("Error while fetching post from database.");
            }
        }

        public async Task<Result<PagedPostsDto>> GetPostsByCategoryAsync(int categoryId, PageInfo pageInfo, SortBy sortBy = SortBy.Date, SortDirection sortDirection = SortDirection.Desc)
        {
            try
            {
                var postsQuery = _context.Posts
                    .Where(p => p.CategoryId == categoryId);

                int totalItems = postsQuery.Count();

                if (totalItems == 0)
                {
                    return Result<PagedPostsDto>.NotFound();
                }

                if (sortBy == SortBy.Title)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts.OrderBy(p => p.Title)
                        : _context.Posts.OrderByDescending(p => p.Title);
                }
                else if (sortBy == SortBy.Views)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts.OrderBy(p => p.ViewCount)
                        : _context.Posts.OrderByDescending(p => p.ViewCount);
                }
                else if (sortBy == SortBy.Likes)
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts.OrderBy(p => p.PostLikes.Count)
                        : _context.Posts.OrderByDescending(p => p.PostLikes.Count);
                }
                else
                {
                    postsQuery = sortDirection == SortDirection.Asc
                        ? _context.Posts.OrderBy(p => p.CreatedAt)
                        : _context.Posts.OrderByDescending(p => p.CreatedAt);
                }

                List<PostDto> postsDtos = await postsQuery
                    .Select(post => new PostDto
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        Likes = post.PostLikes.Count,
                        CommentsCount = post.Comments.Count,
                        ViewCount = post.ViewCount,
                        Edited = post.Edited,
                        CategoryId = post.CategoryId,
                        User = post.User.Adapt<ShortUserDto>()
                    })
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                PagedPostsDto pagedPostsDto = new()
                {
                    Posts = postsDtos,
                    PageInfo = new PageInfo
                    {
                        CurrentPage = pageInfo.CurrentPage,
                        PageSize = pageInfo.PageSize,
                        TotalItems = totalItems
                    }
                };

                return Result<PagedPostsDto>.Success(pagedPostsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching posts from database.");
                return Result<PagedPostsDto>.Failure("Error while fetching posts from database.");
            }
        }

        public async Task<Result<PagedPostsDto>> GetPostsByUserIdAsync(string userId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.Posts
                    .Where(c => c.UserId == userId);

                int totalItems = await query.CountAsync();

                if (totalItems == 0)
                {
                    return Result<PagedPostsDto>.NotFound();
                }

                List<PostDto> postsDto = await query
                    .Select(c => new PostDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        Likes = c.PostLikes.Count,
                        CommentsCount = c.Comments.Count,
                        ViewCount = c.ViewCount,
                        Edited = c.Edited,
                        CategoryId = c.CategoryId,
                        User = c.User.Adapt<ShortUserDto>()
                    })
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                PagedPostsDto pagedPostsDto = new()
                {
                    Posts = postsDto,
                    PageInfo = new PageInfo
                    {
                        CurrentPage = pageInfo.CurrentPage,
                        PageSize = pageInfo.PageSize,
                        TotalItems = totalItems
                    }
                };

                return Result<PagedPostsDto>.Success(pagedPostsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching posts from database.");
                return Result<PagedPostsDto>.Failure("Error while fetching posts from database.");
            }
        }

        public async Task<Result<PostDto>> CreatePostAsync(string userId, CreatePostDto createPostDto)
        {
            try
            {
                Post post = createPostDto.Adapt<Post>();
                post.UserId = userId;
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                PostDto postDto = post.Adapt<PostDto>();
                
                return Result<PostDto>.Success(postDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating post.");
                return Result<PostDto>.Failure("Error while creating post.");
            }
        }

        public async Task<Result<PostDto>> UpdatePostAsync(Post post, UpdatePostDto updatePostDto)
        {
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

                    post.Edited = true;
                    post.EditedAt = DateTime.UtcNow;
                    post.Content = updatePostDto.Content;
                    post.Title = updatePostDto.Title;
                    post.CategoryId = updatePostDto.CategoryId;
                    _context.Posts.Update(post);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Result<PostDto>.Success(post.Adapt<PostDto>());
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    _logger.LogError(ex, "Error while updating post.");
                    return Result<PostDto>.Failure("Error while updating post.");
                }
        }

        public async Task<Result<PostDto>> DeletePostAsync(Post post)
        {
            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return Result<PostDto>.Success(post.Adapt<PostDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting post.");
                return Result<PostDto>.Failure("Error while deleting post.");
            }
        }

    }
}

﻿using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration.UserSecrets;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Dto.Comment;
using WebApi.Dto.Post;
using WebApi.Dto.PostHistory;
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
                // SQL Server specific query

                //var result = _context.Posts
                //    .Where(p => p.Id == postId)
                //    .Select(p => new
                //    {
                //        Post = new
                //        {
                //            p.Id,
                //            p.Title,
                //            p.Content,
                //            User = new ShortUserDto
                //            {
                //                Id = p.User.Id,
                //                UserName = p.User.UserName
                //            }
                //        },
                //        TotalComments = p.Comments.Count(),
                //        Comments = p.Comments
                //            .OrderBy(c => c.Id)
                //            .Skip(pageInfo.Skip)
                //            .Take(pageInfo.PageSize)
                //            .Select(c => new
                //            {
                //                c.Id,
                //                c.Likes,
                //                c.UserId,
                //                User = new ShortUserDto
                //                {
                //                    Id = c.User.Id,
                //                    UserName = c.User.UserName
                //                }
                //            })
                //            .ToList()
                //    }).FirstOrDefaultAsync();

                var post = await _context.Posts
                    .Where(p => p.Id == postId)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync();

                int totalComments = await _context.Comments
                    .Where(c => c.PostId == postId)
                    .CountAsync();

                var comments = await _context.Comments
                    .Where(c => c.PostId == postId)
                    .Include(c => c.User)
                    .OrderBy(c => c.Id)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                PostDto postDto = post.Adapt<PostDto>();
                postDto.Comments = comments.Adapt<List<CommentDto>>();
                List<PostDto> postList = [postDto];

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

                var queryWithPagination = query
                    .Skip(pageInfo.Skip)
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
                post.CreatedAt = DateTime.UtcNow;
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

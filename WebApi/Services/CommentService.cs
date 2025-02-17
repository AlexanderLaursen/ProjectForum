using Common.Dto.Comment;
using Common.Dto.Post;
using Common.Dto.User;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId)
        {
            return await _commentRepository.GetCommentAsync(commentId, userId);
        }

        public async Task<Result<PagedCommentsDto>> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo, string? userId)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(postId, pageInfo, userId);
        }

        public async Task<Result<PagedCommentsDto>> GetCommentsByUsernameAsync(string username, PageInfo pageInfo)
        {
            Result<UserDto> userDto = await _userRepository.GetUserAsync(username);

            if (!userDto.IsSuccess || userDto.Value == null)
            {
                return Result<PagedCommentsDto>.NotFound();
            }

            return await _commentRepository.GetCommentsByUserIdAsync(userDto.Value.Id, pageInfo);
        }

        public async Task<Result<CommentDto>> CreateCommentAsync(string userId, CreateCommentDto createCommentDto)
        {
            return await _commentRepository.CreateCommentAsync(userId, createCommentDto);
        }
      
        public async Task<Result<CommentDto>> UpdateCommentAsync(int commentId, string userId, UpdateCommentDto updateCommentDto)
        {
            Result<Comment> result = await _commentRepository.GetFullCommentAsync(commentId);

            if (!result.IsSuccess)
            {
                return Result<CommentDto>.ConvertDtoError<Comment, CommentDto>(result);
            }

            if (result.Value == null)
            {
                return Result<CommentDto>.NotFound();
            }

            if (result.Value!.UserId != userId)
            {
                return Result<CommentDto>.Unauthorized();
            }

            return await _commentRepository.UpdateCommentAsync(result.Value, updateCommentDto);
        }

        public async Task<Result<CommentDto>> DeleteCommentAsync(int commentId, string userId)
        {
            Result<Comment> result = await _commentRepository.GetFullCommentAsync(commentId);

            if (!result.IsSuccess)
            {
                return Result<CommentDto>.ConvertDtoError<Comment, CommentDto>(result);
            }

            if (result.Value == null)
            {
                return Result<CommentDto>.NotFound();
            }

            if (result.Value.UserId != userId)
            {
                return Result<CommentDto>.Unauthorized();
            }

            return await _commentRepository.DeleteCommentAsync(result.Value);
        }
    }
}

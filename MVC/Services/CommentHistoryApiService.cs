using MVC.Models;
using Common.Models;
using MVC.Repositories;
using MVC.Services.Interfaces;
using Common.Dto.CommentHistory;
using MVC.Repositories.Interfaces;
using Common.Dto.Comment;
using Mapster;

namespace MVC.Services
{
    public class CommentHistoryApiService : CommonApiService, ICommentHistoryApiService
    {

        private readonly IApiRepository _apiRepository;

        public CommentHistoryApiService(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
        }
        public async Task<Result<CommentHistories>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo)
        {
            string urlCommentHistory = UrlFactory($"{COMMENT_HISTORY_PREFIX}/{commentId}", pageInfo);
            string urlComment = UrlFactory($"{COMMENT_PREFIX}/{commentId}");

            var commentHistoryTask = _apiRepository.GetAsync<CommentHistoriesDto>(urlCommentHistory);
            var commentTask = _apiRepository.GetAsync<CommentDto>(urlComment);

            await Task.WhenAll(commentHistoryTask, commentTask);

            Result<CommentHistoriesDto> commentHistoryResult = await commentHistoryTask;
            Result<CommentDto> commentResult = await commentTask;

            if (!commentHistoryResult.IsSuccess || !commentHistoryResult.IsSuccess)
            {
                return Result<CommentHistories>.ConvertDtoError<CommentHistoriesDto, CommentHistories>(commentHistoryResult);
            }

            CommentHistories commentHistories = new CommentHistories
            {
                CommentHistory = commentHistoryResult.Value!.CommentHistories.Adapt<List<CommentHistory>>(),
                Comment = commentResult.Value.Adapt<Comment>(),
                PageInfo = commentHistoryResult.Value.PageInfo,
            };

            return Result<CommentHistories>.Success(commentHistories);
        }
    }
}

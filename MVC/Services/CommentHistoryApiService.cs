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

            if (!commentHistoryTask.Result.IsSuccess || !commentTask.Result.IsSuccess)
            {
                return Result<CommentHistories>.Failure();
            }

            if (commentHistoryTask.Result.Value?.CommentHistories.Count == 0 || string.IsNullOrEmpty(commentTask.Result.Value?.Content))
            {
                return Result<CommentHistories>.NotFound();
            }

            if (commentTask.Result.IsSuccess)
            {
                CommentHistories commentHistories = new CommentHistories
                {
                    Comment = commentTask.Result.Value.Adapt<Comment>(),
                    CommentHistory = commentHistoryTask.Result.Value!.CommentHistories.Adapt<List<CommentHistory>>(),
                    PageInfo = commentHistoryTask.Result.Value.PageInfo,
                };

                return Result<CommentHistories>.Success(commentHistories);
            }
            
            return Result<CommentHistories>.Failure();
        }
    }
}

using MVC.Models;
using Common.Models;
using MVC.Repositories;
using MVC.Services.Interfaces;
using Common.Dto.PostHistory;
using MVC.Repositories.Interfaces;
using Mapster;
using Common.Dto.Post;
using Common.Dto.PostHistory;

namespace MVC.Services
{
    public class PostHistoryApiService : CommonApiService, IPostHistoryApiService
    {
        private readonly IApiRepository _apiRepository;

        public PostHistoryApiService(IApiRepository commonApiService)
        {
            _apiRepository = commonApiService;
        }
        public async Task<Result<PostHistories>> GetPostHistoryAsync(int postId, PageInfo pageInfo)
        {
            string urlPostHistory = UrlFactory($"{POST_HISTORY_PREFIX}/{postId}", pageInfo);
            string urlPost = UrlFactory($"{POST_PREFIX}/{postId}");

            var postHistoryTask = _apiRepository.GetAsync<PostHistoriesDto>(urlPostHistory);
            var postTask = _apiRepository.GetAsync<PostDto>(urlPost);

            await Task.WhenAll(postHistoryTask, postTask);

            Result<PostHistoriesDto> postHistoryResult = await postHistoryTask;
            Result<PostDto> postResult = await postTask;

            if (!postHistoryResult.IsSuccess || !postHistoryResult.IsSuccess)
            {
                return Result<PostHistories>.ConvertDtoError<PostHistoriesDto, PostHistories>(postHistoryResult);
            }

            PostHistories postHistories = new PostHistories
            {
                PostHistory = postHistoryResult.Value!.PostHistories.Adapt<List<PostHistory>>(),
                Post = postResult.Value.Adapt<Post>(),
                PageInfo = postHistoryResult.Value.PageInfo,
            };

            return Result<PostHistories>.Success(postHistories);
        }
    }
}
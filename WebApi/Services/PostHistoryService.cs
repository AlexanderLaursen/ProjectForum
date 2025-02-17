using Common.Dto.Post;
using Common.Dto.PostHistory;
using Common.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class PostHistoryService : IPostHistoryService
    {
        private readonly IPostHistoryRepository _postHistoryRepository;
        private readonly IPostRepository _postRepository;
        public PostHistoryService(IPostHistoryRepository postHistoryRepository, IPostRepository postRepository)
        {
            _postHistoryRepository = postHistoryRepository;
            _postRepository = postRepository;
        }

        public async Task<Result<PostHistoriesDto>> GetPostHistoryAsync(int postId, PageInfo pageInfo)
        {
            return await _postHistoryRepository.GetPostHistoryAsync(postId, pageInfo);
        }
    }
}

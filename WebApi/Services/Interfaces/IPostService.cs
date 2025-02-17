using Common.Dto.Post;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface IPostService
    {
        Task<Result<PostDto>> GetPostAsync(int postId);
    }
}

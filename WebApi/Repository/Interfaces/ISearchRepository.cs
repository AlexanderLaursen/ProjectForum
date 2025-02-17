using WebApi.Models.Ope;
using Common.Models;
using Common.Dto.Search;

namespace WebApi.Repository.Interfaces
{
    public interface ISearchRepository
    {
        public Task<Result<PagedSearchResultDto>> SearchAsync(string searchString, PageInfo pageInfo = null!);
    }
}

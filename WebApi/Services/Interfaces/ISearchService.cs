using Common.Dto.Search;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface ISearchService
    {
        public Task<Result<PagedSearchResultDto>> SearchAsync(string searchString, PageInfo pageInfo);
    }
}

using Common.Dto.Search;
using Common.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public async Task<Result<PagedSearchResultDto>> SearchAsync(string searchString, PageInfo pageInfo)
        {
            return await _searchRepository.SearchAsync(searchString, pageInfo);
        }
    }
}

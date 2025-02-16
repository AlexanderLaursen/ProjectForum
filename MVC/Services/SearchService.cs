using MVC.Models;
using Common.Dto;
using Common.Dto.Search;

namespace MVC.Services
{
    public class SearchService : CommonApiService
    {
        public SearchService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResponse<PaginatedResult<SearchResultDto>>> SearchAsync (string searchString, Common.Models.PageInfo pageInfo)
        {

            string url = ($"Search?searchString={searchString}&page={pageInfo.CurrentPage}&pageSize={pageInfo.PageSize}");

            var result = await GetAsync<PaginatedResult<SearchResultDto>>(url);

            return result;
        }
    }
}


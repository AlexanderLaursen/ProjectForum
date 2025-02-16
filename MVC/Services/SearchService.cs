using MVC.Models;
using Common.Dto;
using Common.Dto.Search;
using MVC.Repositories;

namespace MVC.Services
{
    public class SearchService : ApiRepository
    {
        public SearchService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResponse<PaginatedResult<SearchResultDto>>> SearchAsync (string searchString, Common.Models.PageInfo pageInfo)
        {

            string url = ($"Search?searchString={searchString}&page={pageInfo.CurrentPage}&pageSize={pageInfo.PageSize}");

            var result = await GetAsyncOld<PaginatedResult<SearchResultDto>>(url);

            return result;
        }
    }
}


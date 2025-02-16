using Common.Dto.Category;
using Common.Models;
using Mapster;

namespace MVC.Services
{
    public class CommonApiService
    {
        public const string BASE_URL = "https://localhost:7052/api/v2/";
        public const string COMMENT_HISTORY_PREFIX = "comment-history";
        public const string COMMENT_PREFIX = "comments";
        public const string USER_PREFIX = "User";

        public string UrlFactory(string prefix, PageInfo? pageInfo = null, IEnumerable<string>? queryStrings = null)
        {
            var uriBuilder = new UriBuilder(BASE_URL);
            uriBuilder.Path += prefix;
            var query = new List<string>();

            if (pageInfo != null)
            {
                query.Add($"page={pageInfo.CurrentPage}");
                query.Add($"pageSize={pageInfo.PageSize}");
            }

            if (queryStrings != null && queryStrings.Any())
            {
                query.AddRange(queryStrings);
            }

            uriBuilder.Query = string.Join("&", query);
            return uriBuilder.ToString();
        }

        public Result<TNew> ConvertDto<TOld, TNew>(Result<TOld> result)
        {
            return result.IsSuccess
                ? Result<TNew>.Success(result.Value.Adapt<TNew>())
                : Result<TNew>.ConvertDtoError<TOld, TNew>(result);
        }

        public Result<List<TNew>> ConvertDtoList<TList, TSingle, TNew>(Result<TList> result, List<TSingle> resultList)
        {
            return result.IsSuccess
                ? Result<List<TNew>>.Success(resultList.Adapt<List<TNew>>())
                : Result<List<TNew>>.ConvertDtoError<TList, List<TNew>>(result);
        }
    }
}
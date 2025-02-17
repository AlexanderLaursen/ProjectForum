using Common.Enums;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Models;
using WebApi.Models;
using WebApi.Models.Ope;
using WebApi.Repository.Interfaces;
using Common.Dto.Search;

namespace WebApi.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<SearchRepository> _logger;
        public SearchRepository(DataContext context, ILogger<SearchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PagedSearchResultDto>> SearchAsync(string searchString, PageInfo pageInfo)
        {
            try
            {
                var postQuery = _context.Posts
                    .Where(p => EF.Functions.ToTsVector("english", p.Title + " " + p.Content)
                                .Matches(EF.Functions.PlainToTsQuery("english", searchString)))
                    .Select(p => new SearchResultDto
                    {
                        Id = p.Id,
                        Type = ContentType.Post,
                        Title = p.Title,
                        Content = p.Content
                    });

                var commentQuery = _context.Comments
                    .Where(c => EF.Functions.ToTsVector("english", c.Content)
                                .Matches(EF.Functions.PlainToTsQuery("english", searchString)))
                    .Select(c => new SearchResultDto
                    {
                        Id = c.PostId,
                        Type = ContentType.Comment,
                        Title = "",
                        Content = c.Content
                    });

                string searchStringWithWildcard = $"%{searchString}%";

                var userQuery = _context.Users
                    .Where(u => EF.Functions.ILike(u.UserName!, searchStringWithWildcard))
                    .Select(u => new SearchResultDto
                    {
                        Id = 0,
                        Type = ContentType.User,
                        Title = u.UserName!,
                        Content = ""
                    });

                var combinedQuery = postQuery
                    .Union(commentQuery)
                    .Union(userQuery);

                var totalItems = await combinedQuery.CountAsync();

                var result = await combinedQuery
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                pageInfo.TotalItems = totalItems;

                PagedSearchResultDto paginatedResult = new PagedSearchResultDto
                {
                    PageInfo = pageInfo,
                    SearchResults = result
                };

                return Result<PagedSearchResultDto>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching.");
                return Result<PagedSearchResultDto>.Failure("An error occurred while searching.");
            }
        }
    }
}

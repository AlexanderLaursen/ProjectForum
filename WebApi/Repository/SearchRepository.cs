using Common.Enums;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly DataContext _context;
        public SearchRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResultNew> SearchAsync(string searchString, PageInfo pageInfo = null!)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return OperationResultNew.Fail("Invalid search query.");
            }

            if (pageInfo == null)
            {
                pageInfo = new PageInfo();
            }

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
                    .Where(u => EF.Functions.ILike(u.UserName, searchStringWithWildcard))
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

                PaginatedResult<SearchResultDto> paginatedResult = new(result, pageInfo);

                return OperationResultNew.IsSuccess(paginatedResult);
            }
            catch (Exception)
            {
                return OperationResultNew.Fail("An error occurred while searching.");
            }
        }
    }
}

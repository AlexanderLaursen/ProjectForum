using WebApi.Models;

namespace WebApi.Repository
{
    public interface ISearchRepository
    {
        public Task<OperationResultNew> SearchAsync(string searchString, PageInfo pageInfo = null!);
    }
}

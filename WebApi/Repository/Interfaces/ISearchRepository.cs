using WebApi.Models.Ope;
using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface ISearchRepository
    {
        public Task<OperationResultNew> SearchAsync(string searchString, PageInfo pageInfo = null!);
    }
}

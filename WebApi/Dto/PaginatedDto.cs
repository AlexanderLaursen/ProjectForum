using WebApi.Models;

namespace WebApi.Dto
{
    public class PaginatedDto<T>
    {
        public PageInfo PageInfo { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}

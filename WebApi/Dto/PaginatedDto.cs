using WebApi.Models;

namespace WebApi.Dto
{
    public class PaginatedDto<T>
    {
        public PageInfo PageInfo { get; set; }
        public List<T> Data { get; set; }
    }
}

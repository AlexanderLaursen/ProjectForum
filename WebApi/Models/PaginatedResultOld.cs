using Common.Models;

namespace WebApi.Models
{
    public class PaginatedResultOld<T>
    {
        public List<T> Data { get; set; }
        public PageInfo PageInfo { get; set; }

        public PaginatedResultOld()
        {
            Data = new List<T>();
            PageInfo = new PageInfo();
        }

        public PaginatedResultOld(List<T> data, PageInfo pageInfo)
        {
            Data = data;
            PageInfo = pageInfo;
        }
    }
}
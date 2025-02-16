namespace WebApi.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }
        public PageInfo PageInfo { get; set; }

        public PaginatedResult()
        {
            Data = new List<T>();
            PageInfo = new PageInfo();
        }

        public PaginatedResult(List<T> data, PageInfo pageInfo)
        {
            Data = data;
            PageInfo = pageInfo;
        }
    }
}
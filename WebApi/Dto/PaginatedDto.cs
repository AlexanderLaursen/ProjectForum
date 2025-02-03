namespace WebApi.Dto
{
    public class PaginatedDto<T>
    {
        public int ItemsPerPage { get; set; } = 20;
        public int CurrentPage { get; set; } = 1;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        public IEnumerable<T> Data { get; set; }
    }
}

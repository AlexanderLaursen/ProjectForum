namespace MVC.Models
{
    public class ApiResponse<T>
    {
        public List<T> Content { get; set; }
        public PageInfo PageInfo { get; set; }
        public bool IsSuccess { get; set; }

    }
}

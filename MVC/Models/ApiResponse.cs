namespace MVC.Models
{
    public class ApiResponse<T>
    {
        public T? Content { get; set; }
        public PageInfo PageInfo { get; set; }
        public bool IsSuccess { get; set; }

        public static ApiResponse<T> Success(T content = default!, PageInfo pageInfo = null!)
            => new ApiResponse<T> { IsSuccess = true, Content = content ?? default, PageInfo = pageInfo ?? new PageInfo() };

        public static ApiResponse<T> Fail() => new ApiResponse<T> { IsSuccess = false };
    }
}

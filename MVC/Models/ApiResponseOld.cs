namespace MVC.Models
{
    public class ApiResponseOld<T>
    {
        public List<T> Content { get; set; }
        public PageInfo PageInfo { get; set; }
        public bool IsSuccess { get; set; }

        public static ApiResponseOld<T> Success(List<T> content = default!, PageInfo pageInfo = null!)
            => new ApiResponseOld<T> { IsSuccess = true, Content = content ?? [], PageInfo = pageInfo ?? new PageInfo() };

        public static ApiResponseOld<T> Fail() => new ApiResponseOld<T> { IsSuccess = false };
    }
}

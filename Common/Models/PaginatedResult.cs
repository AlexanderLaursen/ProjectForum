using Common.Enums;

namespace Common.Models
{
    public class PaginatedResult<T> : Result<T>
    {
        public PageInfo PageInfo { get; }

        public PaginatedResult(bool isSuccess, T? value, string? errorMessage, ResultStatus status, PageInfo pageInfo)
            : base(isSuccess, value, errorMessage, status)
        {
            PageInfo = pageInfo;
        }

        public static PaginatedResult<T> Success(T value, PageInfo pageInfo)
            => new(true, value, null, ResultStatus.Ok, pageInfo);

        public static PaginatedResult<T> Failure(string errorMessage, PageInfo pageInfo, ResultStatus status = ResultStatus.Error)
            => new(false, default, errorMessage, status, pageInfo);

        public static PaginatedResult<T> NotFound(string errorMessage = "Resource not found")
            => Failure(errorMessage, new PageInfo(), ResultStatus.NotFound);

        public static PaginatedResult<T> InvalidInput(string errorMessage = "Invalid input data")
            => Failure(errorMessage, new PageInfo(), ResultStatus.InvalidInput);

        public static PaginatedResult<T> Unauthorized(string errorMessage = "Unauthorized")
            => Failure(errorMessage, new PageInfo(), ResultStatus.Unauthorized);
    }
}

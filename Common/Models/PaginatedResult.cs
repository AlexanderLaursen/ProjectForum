using Common.Enums;

namespace Common.Models
{
    public class PaginatedResult<T> : IResult<T>
    {
        public PageInfo PageInfo { get; }

        public bool IsSuccess { get; }

        public T? Value { get; }

        public string? ErrorMessage { get; }

        public ResultStatus Status { get; }

        public PaginatedResult(bool isSuccess, T? value, string? errorMessage, ResultStatus status, PageInfo? pageInfo = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Status = status;
            PageInfo = pageInfo ?? new PageInfo();
        }

        public static PaginatedResult<T> Success(T value, PageInfo pageInfo)
            => new PaginatedResult<T>(true, value, null, ResultStatus.Ok, pageInfo);

        public static PaginatedResult<T> Failure(string errorMessage = "Unknown error occured.", ResultStatus status = ResultStatus.Error)
            => new PaginatedResult<T>(false, default, errorMessage, status);

        public static PaginatedResult<T> NotFound(string errorMessage = "Resource not found.")
            => Failure(errorMessage, ResultStatus.NotFound);

        public static PaginatedResult<T> InvalidInput(string errorMessage = "Invalid input data.")
            => Failure(errorMessage, ResultStatus.InvalidInput);

        public static PaginatedResult<T> Unauthorized(string errorMessage = "Unauthorized.")
            => Failure(errorMessage, ResultStatus.Unauthorized);
    }
}

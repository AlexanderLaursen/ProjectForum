using Common.Enums;
using System.Diagnostics;
using System.Net;

namespace Common.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public ResultStatus Status { get; }

        public Result(bool isSuccess, T? value, string? errorMessage, ResultStatus status)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Status = status;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value, null, ResultStatus.Ok);
        public static Result<T> Failure(string errorMessage = "Unkown error occurred.", ResultStatus status = ResultStatus.Error)
            => new Result<T>(false, default, errorMessage, status);
        public static Result<T> NotFound(string errorMessage = "Resource not found.")
            => Failure(errorMessage, ResultStatus.NotFound);
        public static Result<T> InvalidInput(string errorMessage = "Invalid input data.")
            => Failure(errorMessage, ResultStatus.InvalidInput);
        public static Result<T> Unauthorized(string errorMessage = "Unauthorized.")
            => Failure(errorMessage, ResultStatus.Unauthorized);

        public static Result<TNew> ConvertDtoError<TOld, TNew>(Result<TOld> result)
        {
            switch (result.Status)
            {
                case ResultStatus.NotFound:
                    return Result<TNew>.NotFound(result.ErrorMessage ?? "Resource not found.");
                case ResultStatus.InvalidInput:
                    return Result<TNew>.InvalidInput(result.ErrorMessage ?? "Invalid input data.");
                case ResultStatus.Unauthorized:
                    return Result<TNew>.Unauthorized(result.ErrorMessage ?? "Unauthorized.");
                default:
                    return Result<TNew>.Failure(result.ErrorMessage ?? "Unkown error occurred.");
            }
        }
    }


}

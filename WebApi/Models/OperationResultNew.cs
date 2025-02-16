using Common.Models;

namespace WebApi.Models
{
    public class OperationResultNew<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public PageInfo PageInfo { get; set; }

        public static OperationResultNew<T> IsSuccess(T data) =>
            new OperationResultNew<T> { Success = true, Data = data };

        public static OperationResultNew<T> IsFailure(string errorMessage) =>
            new OperationResultNew<T> { Success = false, ErrorMessage = errorMessage };
    }
}

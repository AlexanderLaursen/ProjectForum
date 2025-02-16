using Common.Enums;

namespace Common.Models
{
    public interface IResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public ResultStatus Status { get; }
    }
}

namespace WebApi.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public object InternalData { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult IsSuccess (Dictionary<string, object> data, object internalData = null)
        {
            return new OperationResult
            {
                Success = true,
                Data = data,
                InternalData = internalData ?? new object()
            };
        }

        public static OperationResult Fail(string errorMessage)
        {
            return new OperationResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}

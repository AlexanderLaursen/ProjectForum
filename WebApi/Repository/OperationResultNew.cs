namespace WebApi.Repository
{
    public class OperationResultNew
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResultNew IsSuccess(object data = null)
        {
            return new OperationResultNew
            {
                Success = true,
                Data = data,
            };
        }

        public static OperationResultNew Fail(string errorMessage = "Unkown error.")
        {
            return new OperationResultNew
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
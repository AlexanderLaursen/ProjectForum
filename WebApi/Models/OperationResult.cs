namespace WebApi.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public object InternalData { get; set; }
        public string ErrorMessage { get; set; }
    }
}

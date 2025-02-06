namespace WebApi.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public string ErrorMessage { get; set; }
    }
}

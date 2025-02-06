namespace MVC.Models
{
    public class LoginResponse
    {
        public BearerToken Token { get; set; }
        public bool IsSuccess { get; set; }
    }
}

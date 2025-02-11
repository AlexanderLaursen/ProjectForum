namespace MVC.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }
        public string Website { get; set; }
        public string ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

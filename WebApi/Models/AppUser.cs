using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class AppUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }

        public string? SmProfilePicture { get; set; }
        public string? MdProfilePicture { get; set; }
        public string? LgProfilePicture { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}

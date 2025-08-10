using Microsoft.AspNetCore.Identity;

namespace demo.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}

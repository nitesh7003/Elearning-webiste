using Microsoft.AspNetCore.Identity;

namespace webapplication.Models
{
       public class ApplicationUser : IdentityUser

    {
        public string FullName { get; set; }

    }
}

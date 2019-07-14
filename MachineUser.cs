using Microsoft.AspNetCore.Identity;

namespace CoreCodeCamp
{
    public class MachineUser  : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
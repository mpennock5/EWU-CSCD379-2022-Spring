using Microsoft.AspNetCore.Identity;

namespace Wordle.Api.Data
{
    public class AppUser : IdentityUser
    {
        //Birthday & MasterOfTheUniverse added as claims also. Check on this
        public string DateOfBirth { get; set; } = null!;
        public bool MasterOfTheUniverse { get; set; } = false;
    }
}
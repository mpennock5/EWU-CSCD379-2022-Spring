using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Wordle.Api.Identity;
public static class Policies
{
    public const string RandomAdmin = "RandomAdmin";
    public const string Over21 = "Over21";
    

    public static void RandomAdminPolicy(AuthorizationPolicyBuilder policy)
    {
        policy.RequireRole(Roles.Admin);
        policy.RequireAssertion(context =>
        {
            var random = context.User.Claims.FirstOrDefault(c => c.Type == Claims.Random);
            if (Double.TryParse(random?.Value, out double result))
            {
                return result > 0.5;
            }
            return false;
        });
    }

    //policy to verify age is 21+
    public static void Over21Policy(AuthorizationPolicyBuilder policy)
    {
        policy.RequireAssertion(context =>
        {
            //get the user and check age
            var userBDay = context.User.Claims.FirstOrDefault(c => c.Type == Claims.DateOfBirth);
            return Is21(userBDay);

        });
    }

   

    //helper method

    private static bool Is21(Claim? userBDay)
    {
        if (userBDay != null)
        {
            var parsedDate = DateTime.Parse(userBDay.Value).AddYears(21);
            if (DateTime.Compare(parsedDate, DateTime.Now) <= 0)
            {
                return true;
            }
        }
        return false;
    }

}
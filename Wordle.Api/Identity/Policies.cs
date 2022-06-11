using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Wordle.Api.Identity;
public static class Policies
{
    public const string RandomAdmin = "RandomAdmin";
    

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
            return over21(userBDay);

        });
    }

    //policy to verify age is 21+ && MasterOfTheUniverse
    public const string MasterOfTheUniverse = "MasterOfTheUniverse";
    public static void MasterOfTheUniversePolicy(AuthorizationPolicyBuilder policy)
    {
        policy.RequireClaim(Claims.MasterOfTheUniverse);
        policy.RequireAssertion(context =>
        {
            var user = context.User.Claims.FirstOrDefault(c => c.Type == Claims.MasterOfTheUniverse);
            if (user is not null)
            {
                bool isMasterOfTheUniverse = Boolean.Parse(user.Value);
                if (isMasterOfTheUniverse)
                {
                    var userBDay = context.User.Claims.FirstOrDefault(c => c.Type == Claims.DateOfBirth);
                    return over21(userBDay);
                }
            }
            return false;
        });
    }

    //helper method
    private static bool over21(Claim? userBDay)
    {
        if (userBDay != null)
        {
            string[] d = userBDay.Value.Split('-');
            DateTime birthday = new DateTime(int.Parse(d[0]), int.Parse(d[1]), int.Parse(d[2]));
            if (birthday.AddYears(21) <= DateTime.Today)
            {
                return true;
            }
        }
        return false;
    }

}
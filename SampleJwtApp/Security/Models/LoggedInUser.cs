using Microsoft.AspNetCore.Identity;

namespace SampleJwtApp.Security.Models;

public class LoggedInUser
{
    public LoggedInUser(IdentityUser user, IList<string> roles)
    {
        User = user;
        Roles = roles;
    }

    public IdentityUser User { get; }
    public IList<string> Roles { get; }
}
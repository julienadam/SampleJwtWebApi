using Microsoft.AspNetCore.Identity;

namespace SampleJwtApp.UserPrefs.Entities
{
    public class UserPreferences
    {
        public int Id { get; set; }
        public double Balance { get; set; }

        public IdentityUser User { get; set; }

    }
}

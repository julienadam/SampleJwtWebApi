
using Microsoft.AspNetCore.Identity;

namespace SampleJwtApp.Security.DataAccess.Repositories
{
    public interface IRepositoryAppUser
    {
        Task<IdentityUser> Create(IdentityUser appuser);
        void Delete(IdentityUser appuser);
        IEnumerable<IdentityUser> GetAll();
        void Update(IdentityUser appuser);
    }
}
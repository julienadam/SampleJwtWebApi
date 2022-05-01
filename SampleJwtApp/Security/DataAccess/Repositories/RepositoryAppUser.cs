using Microsoft.AspNetCore.Identity;

namespace SampleJwtApp.Security.DataAccess.Repositories
{
    public class RepositoryAppUser : IRepositoryAppUser
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public RepositoryAppUser(AppDbContext context, ILogger<RepositoryAppUser> logger)
        {
            _appDbContext = context;
            _logger = logger;
        }

        public async Task<IdentityUser> Create(IdentityUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<IdentityUser>(appuser);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(IdentityUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Remove(appuser);
                    if (obj != null)
                    {
                        _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<IdentityUser> GetAll()
        {
            try
            {
                return _appDbContext.Users.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(IdentityUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Update(appuser);
                    if (obj != null)
                        _appDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

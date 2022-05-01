using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SampleJwtApp.Security.DataAccess.Repositories;

namespace SampleJwtApp.Security.Services
{
    public class ServiceAppUser
    {
        public readonly IRepositoryAppUser _repository;

        public ServiceAppUser(IRepositoryAppUser repository)
        {
            _repository = repository;
        }

        //Create Method 
        public async Task<IdentityUser> AddUser(IdentityUser appUser)
        {
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser));
            }
            else
            {
                return await _repository.Create(appUser);
            }
        }

        public void DeleteUser(string id)

        {
            if (id != null)
            {
                // TODO: use GetById ...
                var obj = _repository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (obj != null)
                {
                    _repository.Delete(obj);
                }
                // TODO : else ?
            }
        }

        public void UpdateUser(string id)

        {
            if (id != null)
            {
                var obj = _repository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (obj != null)
                    _repository.Update(obj);
                // TODO : else  ?
            }
        }

        public IEnumerable<IdentityUser> GetAllUser()
        {
            return _repository.GetAll().ToList();
        }
    }
}

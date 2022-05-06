using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleJwtApp.Catalog.ViewModels;
using SampleJwtApp.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleJwtApp.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet]
        [Route("AdminOnly")]
        [Authorize(Roles = RoleDefinitions.AdministratorRoleName)]
        public IEnumerable<Product> GetAdminOnly()
        {
            return new[]
            {
                new Product
                {
                    Id = 1, Name = "Dual power Exo-Skeleton Mark III", ReleaseDate = new DateTime(2035, 10, 24)
                },
                new Product
                {
                    Id = 2, Name = "Invisibility shield", ReleaseDate = new DateTime(2038, 08, 12)
                }
            };
        }

        [HttpGet]
        [Route("Public")]
        [AllowAnonymous]
        public IEnumerable<Product> GetPublicOnly()
        {
            return new[]
            {
                new Product
                {
                    Id = 1, Name = "Feeble Exo-Skeleton Mark I", ReleaseDate = new DateTime(2025, 10, 24)
                },
            };
        }


        [HttpGet]
        [Route("Basic")]
        [Authorize]
        public IEnumerable<Product> GetBasicProducts()
        {
            return new[]
            {
                new Product
                {
                    Id = 1, Name = "Average Exo-Skeleton Mark II", ReleaseDate = new DateTime(2034, 10, 24)
                },
            };
        }
    }
}
using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HSEPractice2.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly WaterparkContext _db;

        public RoleRepository(WaterparkContext db)
        {
            _db = db; 
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _db.Roles.ToList();
        }
    }
}

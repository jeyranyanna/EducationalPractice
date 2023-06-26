using HSEPractice2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace HSEPractice2.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}

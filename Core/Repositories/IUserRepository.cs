using HSEPractice2.Areas.Identity.Data;
using System.Collections;

namespace HSEPractice2.Core.Repositories
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();

        ApplicationUser GetUser(string id);

        ApplicationUser UpdateUser(ApplicationUser user);
    }
}

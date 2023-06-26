using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Core.Repositories;

namespace HSEPractice2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WaterparkContext _db;
        public UserRepository(WaterparkContext db)
        {
            _db = db;       
        }


        public ICollection<ApplicationUser> GetUsers()
        {
            return _db.Users.ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _db.Update(user);
            _db.SaveChanges();

            return user;
        }
    }
}

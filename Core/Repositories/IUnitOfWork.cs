using Microsoft.AspNetCore.Identity;

namespace HSEPractice2.Core.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }
    }
}

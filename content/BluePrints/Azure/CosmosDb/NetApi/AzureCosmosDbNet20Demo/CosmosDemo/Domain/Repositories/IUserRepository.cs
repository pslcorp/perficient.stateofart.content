using CosmosDemo.Domain.Models;

namespace CosmosDemo.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> List();
        Task<AppUser> GetByEmail(string email);
        Task<AppUser> GetById(string id);
        Task<AppUser> Create(AppUser appUser);
    }
}

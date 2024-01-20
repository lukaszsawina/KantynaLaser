using KantynaLaser.Web.Models;

namespace KantynaLaser.Web.Interfaces;

public interface IUserIdentityRepository : IRepository
{
    Task<UserIdentity> GetIdentityByEmail(string email);
    Task<bool> CreateIdentity(UserIdentity user);
}

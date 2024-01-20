using KantynaLaser.Web.Models;

namespace KantynaLaser.Web.Interfaces;

public interface IUserAccountRepository: IRepository
{
    Task<List<UserAccount>> GetUsers();
    Task<UserAccount> GetUser(Guid id);
    Task<UserAccount> GetUserByEmail(string email);

    Task<bool> CreateUser(UserAccount user);
    Task<bool> UpdateUser(UserAccount user);

    Task<bool> DeleteUser(Guid id);
}

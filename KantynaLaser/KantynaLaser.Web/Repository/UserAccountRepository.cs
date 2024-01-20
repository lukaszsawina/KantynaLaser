using KantynaLaser.Web.Data;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KantynaLaser.Web.Repository;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly DataContext _context;

    public UserAccountRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<UserAccount>> GetUsers()
    {
        return await _context.UserAccount.ToListAsync();
    }

    public async Task<UserAccount> GetUser(Guid id)
    {
        return await _context.UserAccount.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserAccount> GetUserByEmail(string email)
    {
        return await _context.UserAccount.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> CreateUser(UserAccount user)
    {
        await _context.UserAccount.AddAsync(user);
        return await SaveAsync();
    }
    public async Task<bool> UpdateUser(UserAccount user)
    {
        _context.UserAccount.Update(user);
        return await SaveAsync();
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var userToDelete = await _context.UserAccount.FirstOrDefaultAsync(x => x.Id == id);
        _context.UserAccount.Remove(userToDelete);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
}

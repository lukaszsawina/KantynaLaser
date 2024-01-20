using KantynaLaser.Web.Data;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KantynaLaser.Web.Repository;

public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly DataContext _context;

    public UserIdentityRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateIdentity(UserIdentity user)
    {
        await _context.UserIdentity.AddAsync(user);
        return await SaveAsync();
    }

    public async Task<UserIdentity> GetIdentityByEmail(string email)
    {
        return await _context.UserIdentity.FirstOrDefaultAsync(x => x.User.Email == email);
    }

    public async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
}

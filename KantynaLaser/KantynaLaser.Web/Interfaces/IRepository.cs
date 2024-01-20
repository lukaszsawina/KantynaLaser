using Microsoft.EntityFrameworkCore;

namespace KantynaLaser.Web.Interfaces;

public interface IRepository
{
    Task<bool> SaveAsync();
}


using KantynaLaser.Web.Data.Configuration;
using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KantynaLaser.Web.Data;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<UserAccount> UserAccount { get; set; }
	public DbSet<Recipe> Recipe {  get; set; }
    public DbSet<UserIdentity> UserIdentity { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserIdenittyConfiguration());
    }
}

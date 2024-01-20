using KantynaLaser.Web.Data;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KantynaLaser.Web.Repository;

public class RecipeRepository : IRecipeRepository
{
    private readonly DataContext _context;

    public RecipeRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Recipe>> GetRecipes()
    {
        return await _context.Recipe.OrderBy(x => x.CreatedAt).ToListAsync();
    }
    public async Task<Recipe> GetRecipe(Guid id)
    {
        return await _context.Recipe.FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<Recipe>> GetUserRecipes(Guid id)
    {
        return await _context.Recipe.Where(x => x.UserAccountId == id).ToListAsync();
    }
    public async Task<List<Recipe>> GetPublicRecipes()
    {
        return await _context.Recipe.Where(x => x.IsPublic == true).ToListAsync();
    }

    public async Task<bool> CreateRecipe(Recipe recipe)
    {
        await _context.Recipe.AddAsync(recipe);
        return await SaveAsync();
    }
    public async Task<bool> UpdateRecpe(Recipe recipe)
    {
        _context.Recipe.Update(recipe);
        return await SaveAsync();
    }
    public async Task<bool> DeleteRecipe(Recipe recipe)
    {
        _context.Recipe.Remove(recipe);
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
}

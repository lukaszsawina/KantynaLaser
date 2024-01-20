using KantynaLaser.Web.Models;

namespace KantynaLaser.Web.Interfaces;

public interface IRecipeRepository : IRepository
{
    Task<List<Recipe>> GetRecipes();
    Task<Recipe> GetRecipe(Guid id);
    Task<List<Recipe>> GetUserRecipes(Guid id);
    Task<List<Recipe>> GetPublicRecipes();

    Task<bool> CreateRecipe(Recipe recipe);
    Task<bool> UpdateRecpe(Recipe recipe);
    Task<bool> DeleteRecipe(Recipe recipe);
}

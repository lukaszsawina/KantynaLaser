using Microsoft.AspNetCore.Mvc;

namespace KantynaLaser.Web.Models.DTO;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Tool> Tools { get; set; }
    public List<Step> Steps { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan CookingTime { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
}
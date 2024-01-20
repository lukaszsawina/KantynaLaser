using FluentValidation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using static KantynaLaser.Web.Models.UserAccount;

namespace KantynaLaser.Web.Models;

public class Recipe : Entity
{
    public string Title { get; private set; }
    public string IngredientsJson { get; private set; } = "";
    public string ToolsJson { get; private set; } = "";
    public string StepsJson { get; private set; } = "";
    [NotMapped]
    public List<Ingredient> Ingredients
    {
        get => JsonConvert.DeserializeObject<List<Ingredient>>(IngredientsJson);
        private set => IngredientsJson = JsonConvert.SerializeObject(value);
    }
    [NotMapped]
    public List<Tool> Tools
    {
        get => JsonConvert.DeserializeObject<List<Tool>>(ToolsJson);
        private set => ToolsJson = JsonConvert.SerializeObject(value);
    }
    [NotMapped]
    public List<Step> Steps
    {
        get => JsonConvert.DeserializeObject<List<Step>>(StepsJson);
        private set => StepsJson = JsonConvert.SerializeObject(value);
    }
    public TimeSpan PreparationTime { get; private set; }
    public TimeSpan CookingTime { get; private set; }
    public bool IsPublic { get; private set; } = false;

    public Guid UserAccountId { get; set; } 
    public UserAccount User { get; set; } = null!;

    public class RecipeValidator : AbstractValidator<Recipe>
    {
        public RecipeValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CookingTime).NotEmpty();
            RuleFor(x => x.PreparationTime).NotEmpty();
        }
    }

    public Recipe(
        string title, 
        List<Ingredient> ingredients, 
        List<Step> steps, 
        List<Tool> tools, 
        TimeSpan preparationTime, 
        TimeSpan cookingTime, 
        bool isPublic)
    {
        Title = title;
        PreparationTime = preparationTime;
        CookingTime = cookingTime;

        Ingredients = ingredients;
        Steps = steps;
        Tools = tools;

        IsPublic = isPublic;

        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        Validate();
    }

    public Recipe()
    {
        Ingredients = new List<Ingredient>();
        Steps = new List<Step>();
        Tools = new List<Tool>();
    }

    public void Update(
        string title,
        List<Ingredient> ingredients,
        List<Step> steps,
        List<Tool> tools,
        TimeSpan preparationTime,
        TimeSpan cookingTime,
        bool isPublic)
    {
        Title = title;
        PreparationTime = preparationTime;
        CookingTime = cookingTime;

        Ingredients = ingredients;
        Steps = steps;
        Tools = tools;

        IsPublic = isPublic;

        UpdatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var validator = new RecipeValidator();
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException($"Validation failed: {string.Join(", ", validationResult.Errors)}");
        }
    }
}

public record Ingredient(string Name, double Quantity, string Unit);
public record Tool(string Name, int Amount);
public record Step(string Name);


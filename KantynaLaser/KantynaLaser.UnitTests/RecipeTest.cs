using FluentAssertions;
using KantynaLaser.UnitTests.Helper;
using KantynaLaser.Web.Models;
using Xunit;

namespace KantynaLaser.UnitTests;

public class RecipeTest
{
    [Fact]
    public void CreateRecipe_With_ValidData()
    {
        // Arrange
        var title = "Example recipe";
        var ingredients = new List<Ingredient>();
        var steps = new List<Step>();
        var tools = new List<Tool>();
        TimeSpan cookingTime = TimeSpan.FromMinutes(1);
        TimeSpan preparationTime = TimeSpan.FromMinutes(1);
        bool isPublic = true;

        // Act
        var recipe = new Recipe(title, ingredients, steps, tools, preparationTime, cookingTime, isPublic);

        // Assert
        recipe.Should().NotBeNull();
        recipe.Title.Should().Be(title);
        recipe.Ingredients.Should().BeEmpty();
        recipe.Steps.Should().BeEmpty();
        recipe.Tools.Should().BeEmpty();
        recipe.PreparationTime.Should().Be(preparationTime);
        recipe.CookingTime.Should().Be(cookingTime);
        recipe.IsPublic.Should().Be(isPublic);
    }

    public static IEnumerable<object[]> InvalidRecipeData()
    {
        yield return new object[] { "", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), "Validation failed: 'Title' must not be empty." }; 
        yield return new object[] { null, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), "Validation failed: 'Title' must not be empty." };
        yield return new object[] { StringGenerate.GenerateString(201), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), "Validation failed: The length of 'Title' must be 200 characters or fewer. You entered 201 characters." }; 
        yield return new object[] { "Spaghetti Bolognese", null, TimeSpan.FromMinutes(45), "Validation failed: 'Preparation Time' must not be empty." };
        yield return new object[] { "Chicken Alfredo", TimeSpan.FromMinutes(30), null, "Validation failed: 'Cooking Time' must not be empty." };
    }

    [Theory]
    [MemberData(nameof(InvalidRecipeData))]
    public void Try_To_CreateRecipe_With_InvalidData(string title, TimeSpan preparationTime, TimeSpan cookingTime, string exceptionMessage)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(title, new(), new(), new(), preparationTime, cookingTime, true));

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(exceptionMessage);
    }

    [Fact]
    public void UpdateRecipe_With_ValidData()
    {
        // Arrange
        var title = "Example recipe";
        var ingredients = new List<Ingredient>();
        var steps = new List<Step>();
        var tools = new List<Tool>();
        TimeSpan cookingTime = TimeSpan.FromMinutes(1);
        TimeSpan preparationTime = TimeSpan.FromMinutes(1);
        bool isPublic = true;

        var recipe = new Recipe(title, ingredients, steps, tools, preparationTime, cookingTime, isPublic);

        title = "New example recipe";
        cookingTime = TimeSpan.FromMinutes(30);
        preparationTime = TimeSpan.FromMinutes(30);
        isPublic = false;

        // Act
        recipe.Update(title, ingredients, steps, tools, preparationTime, cookingTime, isPublic);

        // Assert
        recipe.Should().NotBeNull();
        recipe.Title.Should().Be(title);
        recipe.Ingredients.Should().BeEmpty();
        recipe.Steps.Should().BeEmpty();
        recipe.Tools.Should().BeEmpty();
        recipe.PreparationTime.Should().Be(preparationTime);
        recipe.CookingTime.Should().Be(cookingTime);
        recipe.IsPublic.Should().Be(isPublic);
    }

    [Theory]
    [MemberData(nameof(InvalidRecipeData))]
    public void Try_To_UpdateRecipe_With_InvalidData(string title, TimeSpan preparationTime, TimeSpan cookingTime, string exceptionMessage)
    {
        // Arrange
        var ingredients = new List<Ingredient>();
        var steps = new List<Step>();
        var tools = new List<Tool>();
        bool isPublic = true;

        var recipe = new Recipe("Example recipe", ingredients, steps, tools, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(30), isPublic);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => recipe.Update(title, new(), new(), new(), preparationTime, cookingTime, true));

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(exceptionMessage);
    }
}

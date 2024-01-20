using FluentAssertions;
using KantynaLaser.IntegrationTests.Helper;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace KantynaLaser.IntegrationTests;

public class RecipeTest
{
    private readonly APIClient _client;
    public RecipeTest()
    {
        _client = new APIClient();
    }

    [Fact]
    public async Task Get_Recipes()
    {
        // Arrange
        await _client.LoginAsync();

        // Act
        var response = await _client.GetAsync("Recipes");

        var jsonResult = await response.Content.ReadAsStringAsync();
        var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipes.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_PublicRecipes()
    {
        // Arrange
        await _client.LoginAsync();

        // Act
        var response = await _client.GetAsync("Recipes/public");

        var jsonResult = await response.Content.ReadAsStringAsync();
        var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipes.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_Recipe_With_ValidData()
    {
        // Arrange
        await _client.LoginAsync();
        var response = await _client.GetAsync("Recipes");

        var jsonResult = await response.Content.ReadAsStringAsync();
        var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult);
        var recipe = recipes.FirstOrDefault();

        // Act
        response = await _client.GetAsync($"Recipes/{recipe.Id}");

        jsonResult = await response.Content.ReadAsStringAsync();
        var actualRecipe = JsonConvert.DeserializeObject<RecipeDto>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualRecipe.Should().NotBeNull();
        actualRecipe.Title.Should().Be(recipe.Title);
        actualRecipe.Ingredients.Should().BeEquivalentTo(recipe.Ingredients);
        actualRecipe.Steps.Should().BeEquivalentTo(recipe.Steps);
        actualRecipe.Tools.Should().BeEquivalentTo(recipe.Tools);
        actualRecipe.PreparationTime.Should().Be(recipe.PreparationTime);
        actualRecipe.CookingTime.Should().Be(recipe.CookingTime);
        actualRecipe.IsPublic.Should().Be(recipe.IsPublic);
    }

    [Fact]
    public async Task Try_To_Get_Recipe_With_InvalidData()
    {
        // Arrange
        await _client.LoginAsync();

        // Act
        var response = await _client.GetAsync("Recipes/550e8400-e29b-41d4-a716-446655440000");
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_User_Recipes_With_ValidData()
    {
        // Arrange
        await _client.LoginAsync();

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == "kowal@wp.pl");

        // Act
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipes.Should().NotBeNull();
    }

    [Fact]
    public async Task Try_To_Get_User_Recipes_With_InvalidData()
    {
        // Arrange
        await _client.LoginAsync();

        // Act
        var response = await _client.GetAsync("Recipes/User/550e8400-e29b-41d4-a716-446655440000");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRecipe_With_ValidData()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };

        // Act
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);

        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var actualRecipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualRecipe.Should().NotBeNull();
        actualRecipe.Title.Should().Be(recipeRequestData.Title);
        actualRecipe.Ingredients.Should().BeEquivalentTo(recipeRequestData.Ingredients);
        actualRecipe.Steps.Should().BeEquivalentTo(recipeRequestData.Steps);
        actualRecipe.Tools.Should().BeEquivalentTo(recipeRequestData.Tools);
        actualRecipe.PreparationTime.Should().Be(recipeRequestData.PreparationTime);
        actualRecipe.CookingTime.Should().Be(recipeRequestData.CookingTime);
        actualRecipe.IsPublic.Should().Be(recipeRequestData.IsPublic);
    }

    public static IEnumerable<object[]> InvalidRecipecsData()
    {
        yield return new object[] { "", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45) };
        yield return new object[] { null, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45) };
        yield return new object[] { StringGenerate.GenerateString(201), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45) };
        yield return new object[] { "Spaghetti Bolognese", null, TimeSpan.FromMinutes(45) };
        yield return new object[] { "Chicken Alfredo", TimeSpan.FromMinutes(30), null };
    }

    [Theory]
    [MemberData(nameof(InvalidRecipecsData))]
    public async Task Try_CreateRecipe_With_InvalidData(string title, TimeSpan preparationTime, TimeSpan cookingTime)
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var recipeRequestData = new
        {
            Title = title,
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = preparationTime,
            CookingTime = cookingTime,
            IsPublic = false
        };

        // Act
        var response = await _client.PostAsync<object>("Recipes", recipeRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRecipe_With_ValidData()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var actualRecipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        var updateRecipeRequestData = new
        {
            Id = actualRecipe.Id,
            Title = "Nowy przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(40),
            CookingTime = TimeSpan.FromMinutes(40),
            IsPublic = false
        };

        // Act
        response = await _client.PutAsync<object>("Recipes", updateRecipeRequestData);

        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        actualRecipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualRecipe.Should().NotBeNull();
        actualRecipe.Title.Should().Be(updateRecipeRequestData.Title);
        actualRecipe.Ingredients.Should().BeEquivalentTo(updateRecipeRequestData.Ingredients);
        actualRecipe.Steps.Should().BeEquivalentTo(updateRecipeRequestData.Steps);
        actualRecipe.Tools.Should().BeEquivalentTo(updateRecipeRequestData.Tools);
        actualRecipe.PreparationTime.Should().Be(updateRecipeRequestData.PreparationTime);
        actualRecipe.CookingTime.Should().Be(updateRecipeRequestData.CookingTime);
        actualRecipe.IsPublic.Should().Be(updateRecipeRequestData.IsPublic);
    }

    [Theory]
    [MemberData(nameof(InvalidRecipecsData))]
    public async Task Try_To_UpdateRecipe_With_InvalidData(string title, TimeSpan preparationTime, TimeSpan cookingTime)
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };

        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        var updateRecipeRequestData = new
        {
            Id = recipe.Id,
            Title = title,
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = preparationTime,
            CookingTime = cookingTime,
            IsPublic = false
        };

        // Act
        response = await _client.PutAsync<object>("Recipes", updateRecipeRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Try_To_UpdateRecipe_With_InvalidUser()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        await _client.LoginAsync();

        var updateRecipeRequestData = new
        {
            Id = recipe.Id,
            Title = "Nowy przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(40),
            CookingTime = TimeSpan.FromMinutes(40),
            IsPublic = false
        };

        // Act
        response = await _client.PutAsync<object>("Recipes", updateRecipeRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Try_To_UpdateRecipe_With_InvalidUserID()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        await _client.LoginAsync();

        var updateRecipeRequestData = new
        {
            Id = "550e8400-e29b-41d4-a716-446655440000",
            Title = "Nowy przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(40),
            CookingTime = TimeSpan.FromMinutes(40),
            IsPublic = false
        };

        // Act
        response = await _client.PutAsync<object>("Recipes", updateRecipeRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRecipe_With_ValidData()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        // Act
        response = await _client.DeleteAsync($"Recipes/{recipe.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Try_To_DeleteRecipe_With_InvalidData()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        // Act
        var response = await _client.DeleteAsync($"Recipes/550e8400-e29b-41d4-a716-446655440000");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Try_To_DeleteRecipe_With_InvalidUser()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };
        await _client.PostAsync<object>("Identity/register", registerRequestData);
        await _client.LoginAsync(registerRequestData.Email, registerRequestData.Password);

        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        var recipeRequestData = new
        {
            Title = "Przykładowy przepis",
            Ingredients = new List<Ingredient>(),
            Tools = new List<Ingredient>(),
            Steps = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(30),
            IsPublic = false
        };
        response = await _client.PostAsync<object>("Recipes", recipeRequestData);
        response = await _client.GetAsync($"Recipes/User/{user.Id}");
        jsonResult = await response.Content.ReadAsStringAsync();
        var recipe = JsonConvert.DeserializeObject<List<RecipeDto>>(jsonResult)?.FirstOrDefault();

        await _client.LoginAsync();

        // Act
        response = await _client.DeleteAsync($"Recipes/{recipe.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

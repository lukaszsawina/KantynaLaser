using FluentAssertions;
using KantynaLaser.IntegrationTests.Helper;
using KantynaLaser.Web.Models.DTO;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace KantynaLaser.IntegrationTests;

public class UserAccountTest
{
    private readonly APIClient _client;
    public UserAccountTest()
    {
        _client = new APIClient();
    }

    [Fact]
    public async Task Get_Users()
    {
        // Arrange
        await _client.LoginAsync();

        // Act
        var response = await _client.GetAsync("Users");

        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_User_With_ValidData()
    {
        // Arrange
        await _client.LoginAsync();
        var response = await _client.GetAsync("Users");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var user = users?.FirstOrDefault(x => x.Email == "kowal@wp.pl");

        // Act
        response = await _client.GetAsync($"Users/{user?.Id}");

        jsonResult = await response.Content.ReadAsStringAsync();
        var actualUser = JsonConvert.DeserializeObject<UserAccountDto>(jsonResult);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualUser.Should().NotBeNull();
        actualUser?.Id.Should().Be(user.Id);
        actualUser?.FirstName.Should().Be(user?.FirstName);
        actualUser?.LastName.Should().Be(user?.LastName);
        actualUser?.Email.Should().Be(user?.Email);
    }

    [Fact]
    public async Task Try_To_GetUser_With_InvalidData()
    {
        // Arrange
        await _client.LoginAsync();
        await _client.GetAsync("Users");

        // Act
        var response = await _client.GetAsync($"Users/550e8400-e29b-41d4-a716-446655440000");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUserAccount_With_ValidData()
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

        var updateRequestData = new
        {
            Id = user.Id,
            FirstName = "ChangedFirstName",
            LastName = "ChangedLastName",
            Email = user.Email,
        };

        // Act
        response = await _client.PutAsync<object>("Users", updateRequestData);

        response = await _client.GetAsync("Users");
        jsonResult = await response.Content.ReadAsStringAsync();
        users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);
        var actualUser = users?.FirstOrDefault(x => x.Email == registerRequestData.Email);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualUser.Should().NotBeNull();
        actualUser?.Id.Should().Be(updateRequestData.Id);
        actualUser?.FirstName.Should().Be(updateRequestData?.FirstName);
        actualUser?.LastName.Should().Be(updateRequestData?.LastName);
        actualUser?.Email.Should().Be(updateRequestData?.Email);
    }

    public static IEnumerable<object[]> InvalidUpdateUserAccountData()
    {
        yield return new object[] { "", "Doe" };
        yield return new object[] { "John", "" };
        yield return new object[] { null, "Doe" };
        yield return new object[] { "John", null };
        yield return new object[] { StringGenerate.GenerateString(201), "Doe" };
        yield return new object[] { "John", StringGenerate.GenerateString(201) };
    }

    [Theory]
    [MemberData(nameof(InvalidUpdateUserAccountData))]
    public async Task Try_To_UpdateUserAccount_With_InvalidData(string firstName, string lastName)
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

        var updateRequestData = new
        {
            Id = user.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = user.Email,
        };

        // Act
        response = await _client.PutAsync<object>("Users", updateRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Try_To_UpdateUserAccount_With_InvalidID()
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

        var updateRequestData = new
        {
            Id = "550e8400-e29b-41d4-a716-446655440000",
            FirstName = "ChangedFirstName",
            LastName = "ChangedLastName",
            Email = user.Email,
        };

        // Act
        response = await _client.PutAsync<object>("Users", updateRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

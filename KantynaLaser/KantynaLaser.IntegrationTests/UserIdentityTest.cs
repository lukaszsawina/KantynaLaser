using FluentAssertions;
using KantynaLaser.IntegrationTests.Helper;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace KantynaLaser.IntegrationTests;

public class UserIdentityTest
{
    private readonly APIClient _client;
    public UserIdentityTest()
    {
        _client = new APIClient();
    }

    [Fact]
    public async Task RegisterUser_With_ValidData()
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com".GetRandomSuffix(),
            Password = "Haslo1234!",
        };

        // Act
        var response = await _client.PostAsync<object>("Identity/register", registerRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUser_With_ValidData_CreateUserAccount()
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
        var response = await _client.GetAsync("Users");

        var jsonResult = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserAccountDto>>(jsonResult);

        var actualUser = users.FirstOrDefault(x => x.Email == registerRequestData.Email);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualUser.Should().NotBeNull();
        actualUser.FirstName.Should().Be(registerRequestData.FirstName);
        actualUser.LastName.Should().Be(registerRequestData.LastName);
        actualUser.Email.Should().Be(registerRequestData.Email);
    }

    public static IEnumerable<object[]> InvalidRegisterData()
    {
        yield return new object[] { "John", "Doe", "kowal@wp.pl", "Haslo1234!", HttpStatusCode.Conflict  };
        yield return new object[] { "John", "", "john.doe@example.com", "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { null, "Doe", "john.doe@example.com", "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { "John", null, "john.doe@example.com", "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { "John", "Doe", "", "Validation failed: 'Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { "John", "Doe", null, "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { "John", "Doe", "invalidEmail", "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { StringGenerate.GenerateString(201), "Doe", "john.doe@example.com", "Haslo1234!", HttpStatusCode.BadRequest };
        yield return new object[] { "John", StringGenerate.GenerateString(201), "john.doe@example.com", "Haslo1234!", HttpStatusCode.BadRequest };
    }

    [Theory]
    [MemberData(nameof(InvalidRegisterData))]
    public async Task Try_To_RegisterUser_With_InvalidData(string firstName, string lastname, string email, string password, HttpStatusCode statusCode)
    {
        // Arrange
        var registerRequestData = new
        {
            FirstName = firstName,
            LastName = lastname,
            Email = email,
            Password = password,
        };

        // Act
        var response = await _client.PostAsync<object>("Identity/register", registerRequestData);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(statusCode);
    }

    [Fact]
    public async Task LoginUser_With_ValidData()
    {
        // Act
        var response = await _client.LoginAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("example@example.com", "Haslo1234!", HttpStatusCode.NotFound)]
    [InlineData("kowal@wp.pl", "Niepoprawne!", HttpStatusCode.BadRequest)]
    public async Task Try_To_LoginUser_With_InvalidData(string email, string password, HttpStatusCode statusCode)
    {
        // Act
        var response = await _client.LoginAsync(email, password);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(statusCode);
    }
}

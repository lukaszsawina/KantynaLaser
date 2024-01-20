using FluentAssertions;
using KantynaLaser.UnitTests.Helper;
using KantynaLaser.Web.Models;
using Xunit;

namespace KantynaLaser.UnitTests;

public class UserIdentityTest
{
    [Fact]
    public void CreateUserIdentity_With_ValidData()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var password = "Haslo1234!";

        // Act
        var userIdentity = new UserIdentity(firstName, lastName, email, password);

        // Assert
        userIdentity.Should().NotBeNull();
        userIdentity.User.Should().NotBeNull();
        userIdentity.Password.Should().Be(password);
    }
    public static IEnumerable<object[]> InvalidCreateUserIdentityData()
    {
        yield return new object[] { "", "Validation failed: 'Password' must not be empty." };
        yield return new object[] { null, "Validation failed: 'Password' must not be empty." };
        yield return new object[] { StringGenerate.GenerateString(201), "Validation failed: The length of 'Password' must be 200 characters or fewer. You entered 201 characters." };
    }

    [Theory]
    [MemberData(nameof(InvalidCreateUserIdentityData))]
    public void Try_To_CreateUserAccount_With_InvalidData(string password, string exceptionMessage)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";

        // Act
        var exception = Assert.Throws<ArgumentException>(() => new UserIdentity(firstName, lastName, email, password));

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(exceptionMessage);
    }
}

using FluentAssertions;
using KantynaLaser.UnitTests.Helper;
using KantynaLaser.Web.Models;
using Xunit;

namespace KantynaLaser.UnitTests;

public class UserAccountTest
{
    [Fact]
    public void CreateUserAccount_With_ValidData()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";

        // Act
        var createdUser = new UserAccount(firstName, lastName, email);

        // Assert
        createdUser.Should().NotBeNull();
        createdUser.FirstName.Should().Be(firstName);
        createdUser.LastName.Should().Be(lastName);
        createdUser.Email.Should().Be(email);
    }

    public static IEnumerable<object[]> InvalidCreateUserAccountData()
    {
        yield return new object[] { "", "Doe", "john.doe@example.com", "Validation failed: 'First Name' must not be empty." };
        yield return new object[] { "John", "", "john.doe@example.com", "Validation failed: 'Last Name' must not be empty." };
        yield return new object[] { null, "Doe", "john.doe@example.com", "Validation failed: 'First Name' must not be empty." };
        yield return new object[] { "John", null, "john.doe@example.com", "Validation failed: 'Last Name' must not be empty." };
        yield return new object[] { "John", "Doe", "", "Validation failed: 'Email' is not a valid email address., 'Email' must not be empty." };
        yield return new object[] { "John", "Doe", null, "Validation failed: 'Email' must not be empty." };
        yield return new object[] { "John", "Doe", "invalidEmail", "Validation failed: 'Email' is not a valid email address." };
        yield return new object[] { StringGenerate.GenerateString(201), "Doe", "john.doe@example.com", "Validation failed: The length of 'First Name' must be 200 characters or fewer. You entered 201 characters." };
        yield return new object[] { "John", StringGenerate.GenerateString(201), "john.doe@example.com", "The length of 'Last Name' must be 200 characters or fewer. You entered 201 characters." };
    }

    [Theory]
    [MemberData(nameof(InvalidCreateUserAccountData))]
    public void Try_To_CreateUserAccount_With_InvalidData(string firstName, string lastName, string email, string exceptionMessage)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => new UserAccount(firstName, lastName, email));

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(exceptionMessage);
    }

    [Fact]
    public void UpdateAccount_With_ValidData()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var user = new UserAccount(firstName, lastName, email);

        // Act
        firstName = "Alice";
        lastName = "Smith";
        user.Update(firstName, lastName);

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
    }

    public static IEnumerable<object[]> InvalidUpdateUserAccountData()
    {
        yield return new object[] { "", "Doe", "Validation failed: 'First Name' must not be empty." };
        yield return new object[] { "John", "", "Validation failed: 'Last Name' must not be empty." };
        yield return new object[] { null, "Doe", "Validation failed: 'First Name' must not be empty." };
        yield return new object[] { "John", null, "Validation failed: 'Last Name' must not be empty." };
        yield return new object[] { StringGenerate.GenerateString(201), "Doe", "Validation failed: The length of 'First Name' must be 200 characters or fewer. You entered 201 characters." };
        yield return new object[] { "John", StringGenerate.GenerateString(201), "The length of 'Last Name' must be 200 characters or fewer. You entered 201 characters." };
    }

    [Theory]
    [MemberData(nameof(InvalidUpdateUserAccountData))]
    public void Try_To_UpdateUserAccount_With_InvalidData(string firstName, string lastName, string exceptionMessage)
    {
        // Arrange
        var user = new UserAccount("John", "Doe", "john.doe@example.com");

        // Act
        var exception = Assert.Throws<ArgumentException>(() => user.Update(firstName, lastName));

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(exceptionMessage);
    }
}

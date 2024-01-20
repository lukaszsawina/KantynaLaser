using FluentValidation;
using Microsoft.Win32;
using static KantynaLaser.Web.Models.UserAccount;

namespace KantynaLaser.Web.Models;

public class UserIdentity : Entity
{
    public string Password { get; set; }
    public UserAccount? User { get; set; }

    public class UserIdentityValidator : AbstractValidator<UserIdentity>
    {
        public UserIdentityValidator()
        {
            RuleFor(identity => identity.Password).NotEmpty().MaximumLength(200);
        }
    }

    public UserIdentity(string firstName, string lastName, string email, string password)
    {
        Password = password;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        User = new UserAccount(firstName, lastName, email);

        Validate();
    }

    public UserIdentity()
    {
        
    }

    private void Validate()
    {
        var validator = new UserIdentityValidator();
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException($"Validation failed: {string.Join(", ", validationResult.Errors)}");
        }
    }
}

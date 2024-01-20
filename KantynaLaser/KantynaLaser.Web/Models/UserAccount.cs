using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection.Metadata;

namespace KantynaLaser.Web.Models;

public class UserAccount : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set;}
    public string Email { get; private set; }
    public ICollection<Recipe> Recipes { get; } = new List<Recipe>();
    public Guid UserIdentityId { get; set; }
    public UserIdentity UserIdentity { get; set; } = null!;
    public class UserAccountValidator : AbstractValidator<UserAccount>
    {
        public UserAccountValidator()
        {
            RuleFor(person => person.FirstName).NotEmpty().MaximumLength(200);
            RuleFor(person => person.LastName).NotEmpty().MaximumLength(200);
            RuleFor(person => person.Email).EmailAddress().NotEmpty();
        }
    }

    public UserAccount(string firstName, string lastname, string email)
    {
        FirstName = firstName;
        LastName = lastname;
        Email = email;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        Validate();
    }

    public UserAccount() {}


    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var validator = new UserAccountValidator();
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException($"Validation failed: {string.Join(", ", validationResult.Errors)}");
        }
    }
}
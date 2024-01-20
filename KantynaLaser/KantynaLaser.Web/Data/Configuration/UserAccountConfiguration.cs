using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace KantynaLaser.Web.Data.Configuration;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.Id).HasColumnName("UA_Id");
        builder.Property(ua => ua.FirstName).HasColumnName("UA_Firstname");
        builder.Property(ua => ua.LastName).HasColumnName("UA_LastName");
        builder.Property(ua => ua.Email).HasColumnName("UA_Email");
        builder.Property(ua => ua.CreatedAt).HasColumnName("UA_CreatedAt");
        builder.Property(ua => ua.UpdatedAt).HasColumnName("UA_UpdatedAt");
        builder.Property(ua => ua.UserIdentityId).HasColumnName("UA_UserIdentityId");
        builder.HasMany(ua => ua.Recipes)
        .WithOne(r => r.User)
        .HasForeignKey(r => r.UserAccountId)
        .IsRequired();
    }
}

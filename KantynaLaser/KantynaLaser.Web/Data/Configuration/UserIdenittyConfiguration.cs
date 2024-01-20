using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace KantynaLaser.Web.Data.Configuration;

public class UserIdenittyConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.HasKey(ui =>  ui.Id);
        builder.Property(ui => ui.Id).HasColumnName("UI_Id");
        builder.Property(ui => ui.Password).HasColumnName("UI_Password");
        builder.Property(ui => ui.CreatedAt).HasColumnName("UI_CreatedAt");
        builder.Property(ui => ui.UpdatedAt).HasColumnName("UI_UpdatedAt");

        builder.HasOne(ui => ui.User)
            .WithOne(ua => ua.UserIdentity)
            .HasForeignKey<UserAccount>(ua => ua.UserIdentityId)
            .IsRequired();
    }
}

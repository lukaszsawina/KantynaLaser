using KantynaLaser.Web.Helper;
using KantynaLaser.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace KantynaLaser.Web.Data.Configuration;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("R_Id");
        builder.Property(r => r.Title).HasColumnName("R_Title");
        builder.Property(r => r.IngredientsJson).HasColumnName("R_Ingredients").HasColumnType("nvarchar(max)");
        builder.Property(r => r.ToolsJson).HasColumnName("R_Tools").HasColumnType("nvarchar(max)");
        builder.Property(r => r.StepsJson).HasColumnName("R_Steps").HasColumnType("nvarchar(max)");
        builder.Property(r => r.PreparationTime).HasColumnName("R_PreparationTime");
        builder.Property(r => r.CookingTime).HasColumnName("R_CookingTime");
        builder.Property(r => r.CreatedAt).HasColumnName("R_CreatedAt");
        builder.Property(r => r.UpdatedAt).HasColumnName("R_UpdatedAt");
        builder.Property(r => r.IsPublic).HasColumnName("R_IsPublic");
        builder.Property(r => r.UserAccountId).HasColumnName("R_UserAccountId");
        builder.HasOne(r => r.User)
        .WithMany(ua => ua.Recipes)
        .HasForeignKey(r => r.UserAccountId)
        .IsRequired();
    }
}

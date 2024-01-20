﻿// <auto-generated />
using System;
using KantynaLaser.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231230235640_Recipe-update")]
    partial class Recipeupdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KantynaLaser.Web.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("R_Id");

                    b.Property<TimeSpan>("CookingTime")
                        .HasColumnType("time")
                        .HasColumnName("R_CookingTime");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("R_CreatedAt");

                    b.Property<string>("IngredientsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("R_Ingredients");

                    b.Property<TimeSpan>("PreparationTime")
                        .HasColumnType("time")
                        .HasColumnName("R_PreparationTime");

                    b.Property<Guid>("R_UA_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StepsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("R_Steps");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("R_Title");

                    b.Property<string>("ToolsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("R_Tools");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("R_UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("R_UA_ID");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UA_Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("UA_CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UA_Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UA_Firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UA_LastName");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("UA_UpdatedAt");

                    b.Property<Guid>("UserIdentityId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UA_UserIdentityId");

                    b.HasKey("Id");

                    b.HasIndex("UserIdentityId")
                        .IsUnique();

                    b.ToTable("UserAccount");
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.UserIdentity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UI_Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("UI_CreatedAt");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UI_Password");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("UI_UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("UserIdentity");
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.Recipe", b =>
                {
                    b.HasOne("KantynaLaser.Web.Models.UserAccount", null)
                        .WithMany("Recipes")
                        .HasForeignKey("R_UA_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.UserAccount", b =>
                {
                    b.HasOne("KantynaLaser.Web.Models.UserIdentity", "UserIdentity")
                        .WithOne("User")
                        .HasForeignKey("KantynaLaser.Web.Models.UserAccount", "UserIdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.UserAccount", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("KantynaLaser.Web.Models.UserIdentity", b =>
                {
                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}

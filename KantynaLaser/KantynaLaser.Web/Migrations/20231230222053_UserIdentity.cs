using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UA_ID",
                table: "UserAccount",
                newName: "UA_Id");

            migrationBuilder.RenameColumn(
                name: "R_ID",
                table: "Recipe",
                newName: "R_Id");

            migrationBuilder.AddColumn<Guid>(
                name: "UserIdentityId",
                table: "UserAccount",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserIdentity",
                columns: table => new
                {
                    UI_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UI_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UI_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UI_UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentity", x => x.UI_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_UserIdentityId",
                table: "UserAccount",
                column: "UserIdentityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_UserIdentity_UserIdentityId",
                table: "UserAccount",
                column: "UserIdentityId",
                principalTable: "UserIdentity",
                principalColumn: "UI_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_UserIdentity_UserIdentityId",
                table: "UserAccount");

            migrationBuilder.DropTable(
                name: "UserIdentity");

            migrationBuilder.DropIndex(
                name: "IX_UserAccount_UserIdentityId",
                table: "UserAccount");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "UserAccount");

            migrationBuilder.RenameColumn(
                name: "UA_Id",
                table: "UserAccount",
                newName: "UA_ID");

            migrationBuilder.RenameColumn(
                name: "R_Id",
                table: "Recipe",
                newName: "R_ID");
        }
    }
}

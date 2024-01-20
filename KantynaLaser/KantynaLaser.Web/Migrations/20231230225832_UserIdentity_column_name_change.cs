using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentity_column_name_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_UserIdentity_UserIdentityId",
                table: "UserAccount");

            migrationBuilder.RenameColumn(
                name: "UserIdentityId",
                table: "UserAccount",
                newName: "UA_UserIdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_UserIdentityId",
                table: "UserAccount",
                newName: "IX_UserAccount_UA_UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_UserIdentity_UA_UserIdentityId",
                table: "UserAccount",
                column: "UA_UserIdentityId",
                principalTable: "UserIdentity",
                principalColumn: "UI_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_UserIdentity_UA_UserIdentityId",
                table: "UserAccount");

            migrationBuilder.RenameColumn(
                name: "UA_UserIdentityId",
                table: "UserAccount",
                newName: "UserIdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_UA_UserIdentityId",
                table: "UserAccount",
                newName: "IX_UserAccount_UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_UserIdentity_UserIdentityId",
                table: "UserAccount",
                column: "UserIdentityId",
                principalTable: "UserIdentity",
                principalColumn: "UI_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

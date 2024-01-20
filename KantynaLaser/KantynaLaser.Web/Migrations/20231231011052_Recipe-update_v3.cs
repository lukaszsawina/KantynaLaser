using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    /// <inheritdoc />
    public partial class Recipeupdate_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_UserAccount_UserAccountId",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Recipe",
                newName: "R_UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_UserAccountId",
                table: "Recipe",
                newName: "IX_Recipe_R_UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_UserAccount_R_UserAccountId",
                table: "Recipe",
                column: "R_UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UA_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_UserAccount_R_UserAccountId",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "R_UserAccountId",
                table: "Recipe",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_R_UserAccountId",
                table: "Recipe",
                newName: "IX_Recipe_UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_UserAccount_UserAccountId",
                table: "Recipe",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UA_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

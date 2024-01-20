using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    /// <inheritdoc />
    public partial class Recipeupdatev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "R_IsPublic",
                table: "Recipe",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "R_IsPublic",
                table: "Recipe");
        }
    }
}

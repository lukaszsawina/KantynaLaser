using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KantynaLaser.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UA_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UA_Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UA_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UA_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UA_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UA_UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UA_ID);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    R_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    R_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R_Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R_Tools = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R_PreparationTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    R_CookingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    R_Steps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R_UA_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    R_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    R_UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.R_ID);
                    table.ForeignKey(
                        name: "FK_Recipe_UserAccount_R_UA_ID",
                        column: x => x.R_UA_ID,
                        principalTable: "UserAccount",
                        principalColumn: "UA_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_R_UA_ID",
                table: "Recipe",
                column: "R_UA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "UserAccount");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanHofmeyerAdmin.Migrations
{
    /// <inheritdoc />
    public partial class MakeColumnsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
           name: "AboutSection",
           table: "Projects",
           nullable: true,
           oldClrType: typeof(string),
           oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "KeyGoals",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ShortSummary",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

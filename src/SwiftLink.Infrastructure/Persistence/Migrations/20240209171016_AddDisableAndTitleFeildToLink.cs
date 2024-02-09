using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisableAndTitleFeildToLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BASE");

            migrationBuilder.RenameTable(
                name: "Subscriber",
                schema: "Base",
                newName: "Subscriber",
                newSchema: "BASE");

            migrationBuilder.RenameTable(
                name: "LinkVisit",
                schema: "Base",
                newName: "LinkVisit",
                newSchema: "BASE");

            migrationBuilder.RenameTable(
                name: "Link",
                schema: "Base",
                newName: "Link",
                newSchema: "BASE");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                schema: "BASE",
                table: "Link",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "BASE",
                table: "Link",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                schema: "BASE",
                table: "Link");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "BASE",
                table: "Link");

            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.RenameTable(
                name: "Subscriber",
                schema: "BASE",
                newName: "Subscriber",
                newSchema: "Base");

            migrationBuilder.RenameTable(
                name: "LinkVisit",
                schema: "BASE",
                newName: "LinkVisit",
                newSchema: "Base");

            migrationBuilder.RenameTable(
                name: "Link",
                schema: "BASE",
                newName: "Link",
                newSchema: "Base");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailPropertyOnSubscriber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "BASE",
                table: "Subscriber",
                type: "varchar(75)",
                unicode: false,
                maxLength: 75,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "BASE",
                table: "Subscriber");
        }
    }
}

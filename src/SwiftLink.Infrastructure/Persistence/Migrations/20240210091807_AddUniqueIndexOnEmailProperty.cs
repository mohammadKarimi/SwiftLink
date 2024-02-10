using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnEmailProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_Email",
                schema: "BASE",
                table: "Subscriber",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriber_Email",
                schema: "BASE",
                table: "Subscriber");
        }
    }
}

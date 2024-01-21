using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexShortCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Link_ShortCode",
                schema: "Base",
                table: "Link",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Link_ShortCode",
                schema: "Base",
                table: "Link");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsAsValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "BASE",
                table: "Link");

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "BASE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    LinkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Link_LinkId",
                        column: x => x.LinkId,
                        principalSchema: "BASE",
                        principalTable: "Link",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LinkId",
                schema: "BASE",
                table: "Tag",
                column: "LinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tag",
                schema: "BASE");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                schema: "BASE",
                table: "Link",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

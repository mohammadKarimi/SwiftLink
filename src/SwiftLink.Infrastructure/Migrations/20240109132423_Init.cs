using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.CreateTable(
                name: "Subscriber",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Subscriber", x => x.Id);
                },
                comment: "Only these subscribers are allowed to insert a URL to obtain a shorter one.");

            migrationBuilder.CreateTable(
                name: "Link",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    ShortCode = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    OriginalUrl = table.Column<string>(type: "varchar(1500)", unicode: false, maxLength: 1500, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Link", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Link_Subscriber_SubscriberId",
                        column: x => x.SubscriberId,
                        principalSchema: "Base",
                        principalTable: "Subscriber",
                        principalColumn: "Id");
                },
                comment: "Stores Original links and generated shortCode.");

            migrationBuilder.CreateTable(
                name: "LinkVisit",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientMetaData = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_LinkVisit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkVisit_Link_LinkId",
                        column: x => x.LinkId,
                        principalSchema: "Base",
                        principalTable: "Link",
                        principalColumn: "Id");
                },
                comment: "analytics, providing insights into the number of users who clicked on a shortened link.");

            migrationBuilder.CreateIndex(
                name: "IX_Link_SubscriberId",
                schema: "Base",
                table: "Link",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkVisit_LinkId",
                schema: "Base",
                table: "LinkVisit",
                column: "LinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkVisit",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Link",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Subscriber",
                schema: "Base");
        }
    }
}

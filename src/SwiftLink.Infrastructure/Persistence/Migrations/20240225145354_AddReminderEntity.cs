using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reminder",
                schema: "BASE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkId = table.Column<int>(type: "int", nullable: false),
                    TryCount = table.Column<byte>(type: "tinyint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RemindTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Reminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminder_Link_LinkId",
                        column: x => x.LinkId,
                        principalSchema: "BASE",
                        principalTable: "Link",
                        principalColumn: "Id");
                },
                comment: "stores information about reminders that will warn about expirationDate of a link");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_LinkId",
                schema: "BASE",
                table: "Reminder",
                column: "LinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminder",
                schema: "BASE");
        }
    }
}

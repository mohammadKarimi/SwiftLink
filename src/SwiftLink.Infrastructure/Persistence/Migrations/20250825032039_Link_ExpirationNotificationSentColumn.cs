using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Link_ExpirationNotificationSentColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                schema: "BASE",
                table: "Tag",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "ExpirationNotificationSent",
                schema: "BASE",
                table: "Link",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationNotificationSent",
                schema: "BASE",
                table: "Link");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                schema: "BASE",
                table: "Tag",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}

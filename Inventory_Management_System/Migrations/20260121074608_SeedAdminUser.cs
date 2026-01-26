using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "email", "first_name", "last_name", "passwordhash", "role", "username" },
                values: new object[] { 999, "admin@system.local", "System", "Admin", "$2a$11$3.eML3mOt1FZuG0nYQVXEeTVrCIoRCgeQuEQQhJ5T7ncaQA3RAww.", "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: 999);
        }
    }
}

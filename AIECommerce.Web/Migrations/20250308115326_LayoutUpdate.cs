using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIECommerce.Web.Migrations
{
    /// <inheritdoc />
    public partial class LayoutUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "4bd8963c-63a4-4b2c-bdb2-7712df34be62");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "9e712ac4-4a07-44b5-9267-8ed90e22cf7e");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "a16c964f-b24b-4515-83bf-d0a7eefd89c0");

            migrationBuilder.InsertData(
                table: "aspnetroles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0004a19b-0518-40f0-be8b-a65997d730f6", null, "User", "USER" },
                    { "403c62a3-cf9d-47fd-9fa5-a15da215d6e9", null, "Editor", "EDITOR" },
                    { "46b4b665-472b-490b-b2b7-33e0952a2985", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "0004a19b-0518-40f0-be8b-a65997d730f6");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "403c62a3-cf9d-47fd-9fa5-a15da215d6e9");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "46b4b665-472b-490b-b2b7-33e0952a2985");

            migrationBuilder.InsertData(
                table: "aspnetroles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4bd8963c-63a4-4b2c-bdb2-7712df34be62", null, "Editor", "EDITOR" },
                    { "9e712ac4-4a07-44b5-9267-8ed90e22cf7e", null, "Admin", "ADMIN" },
                    { "a16c964f-b24b-4515-83bf-d0a7eefd89c0", null, "User", "USER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebShop.Migrations
{
    public partial class SeedProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e71d461-63e3-4aa5-be93-d701a5a1f913",
                column: "ConcurrencyStamp",
                value: "07f19f8f-b695-4abb-afcf-9d0d53ad0ad4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6217999e-a9fb-448b-b163-e2305fc44f50",
                column: "ConcurrencyStamp",
                value: "17407afb-db55-409e-a9f7-448322dc3049");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66412151-dd0c-4b69-82c8-0f4256e78f00",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e609842a-b15c-4c2a-b502-965bfb40f545", "AQAAAAEAACcQAAAAEPOv+/X1QbOIlpWTLlowsksmnRGxsU0XK5MBrTr+m4r0n1yQHRtYHRbLEUhmzmYCnw==", "de788cc1-6788-4881-8286-6e06971d9bf8" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Image", "InStock", "Price", "Sku", "Title" },
                values: new object[,]
                {
                    { 101, "High in protein", null, 30m, 6.90m, "S006", "Yogurtos" },
                    { 102, "Halves", null, 40m, 3.90m, "S009", "Almie" },
                    { 103, "wink wink", null, 50m, 1.90m, "S206", "Light" },
                    { 104, "Diet", null, 20m, 22.90m, "S906", "Meat" },
                    { 105, "Chick and classy look", null, 50m, 70.90m, "S216", "Rings" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e71d461-63e3-4aa5-be93-d701a5a1f913",
                column: "ConcurrencyStamp",
                value: "4b6cca20-371e-4c74-b1af-54ee0ef73960");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6217999e-a9fb-448b-b163-e2305fc44f50",
                column: "ConcurrencyStamp",
                value: "86f6036f-bcfa-4a9a-8497-92e577a91cab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66412151-dd0c-4b69-82c8-0f4256e78f00",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f9fc699-3d43-4064-96cb-c642a2385bdc", "AQAAAAEAACcQAAAAEADiJPWNlntWsq4u3n/YvVc8d3l6g7//fh0pIH9I961zag6v0eOCU/0Ba8pHVCWDGg==", "193c6328-b929-487d-879d-a46f2db36c65" });
        }
    }
}

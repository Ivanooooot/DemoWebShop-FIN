using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebShop.Migrations
{
    public partial class NC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e71d461-63e3-4aa5-be93-d701a5a1f913",
                column: "ConcurrencyStamp",
                value: "ec51115c-3cd8-41fc-86ec-97333545c201");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6217999e-a9fb-448b-b163-e2305fc44f50",
                column: "ConcurrencyStamp",
                value: "2d5f1075-233d-41ca-8d41-d85afa509782");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66412151-dd0c-4b69-82c8-0f4256e78f00",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57917328-1dba-4f3c-83d4-0fff362a4931", "AQAAAAEAACcQAAAAEGv+tRCPox2OS0L2lkcTaZ29hpL3PrTQkHdOlLHmIDcJWKHeN/kt/7YGPEhXdU6svA==", "843f7d19-67bc-461f-ad37-af8cfc96e9c7" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Mliječni proizvodi");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Domaći proizvodi");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Proteinska hrana");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title",
                value: "Hrana za kiućne ljubimce");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title",
                value: "Nakit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Milk");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Domestic");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Protein food");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title",
                value: "Pets");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title",
                value: "Jewellery");
        }
    }
}

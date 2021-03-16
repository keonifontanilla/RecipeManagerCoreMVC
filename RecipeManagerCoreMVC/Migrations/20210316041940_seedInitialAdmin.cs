using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeManagerCoreMVC.Migrations
{
    public partial class seedInitialAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AboutMe", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FacebookLink", "FirstName", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PinterestLink", "SecurityStamp", "TwitterLink", "TwoFactorEnabled", "UserName" },
                values: new object[] { "42f23ef6-3027-4c2b-aec0-62a7677b6597", null, 0, "6c760586-16d5-4777-93cd-4e422e59126f", "admin@g.com", true, null, null, null, null, false, null, "ADMIN@G.COM", "ADMIN", "AQAAAAEAACcQAAAAEHzqpI0NchsYD76AjLX26j6Z4+yhCG07L4dUeN9/RGTvxcwcGd0bslfqss4GdFbOJQ==", null, false, null, "3b881d39-046e-45f7-a250-ab5ffdcca10e", null, false, "admin" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2021, 3, 15, 18, 19, 40, 442, DateTimeKind.Local).AddTicks(3503));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2021, 3, 15, 18, 19, 40, 442, DateTimeKind.Local).AddTicks(9242));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2021, 3, 15, 18, 19, 40, 442, DateTimeKind.Local).AddTicks(9254));

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "IsAdmin", "true", "42f23ef6-3027-4c2b-aec0-62a7677b6597" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "42f23ef6-3027-4c2b-aec0-62a7677b6597");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2021, 2, 5, 11, 58, 18, 665, DateTimeKind.Local).AddTicks(1206));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2021, 2, 5, 11, 58, 18, 666, DateTimeKind.Local).AddTicks(1235));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2021, 2, 5, 11, 58, 18, 666, DateTimeKind.Local).AddTicks(1249));
        }
    }
}

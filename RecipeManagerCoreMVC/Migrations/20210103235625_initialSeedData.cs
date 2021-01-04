using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeManagerCoreMVC.Migrations
{
    public partial class initialSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CreatedDate", "RecipeDescription", "RecipeName", "RecipeType", "UpdatedDated" },
                values: new object[] { 1, new DateTime(2021, 1, 3, 13, 56, 24, 600, DateTimeKind.Local).AddTicks(5948), null, "Recipe1", 0, null });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CreatedDate", "RecipeDescription", "RecipeName", "RecipeType", "UpdatedDated" },
                values: new object[] { 2, new DateTime(2021, 1, 3, 13, 56, 24, 601, DateTimeKind.Local).AddTicks(5797), null, "Recipe2", 0, null });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CreatedDate", "RecipeDescription", "RecipeName", "RecipeType", "UpdatedDated" },
                values: new object[] { 3, new DateTime(2021, 1, 3, 13, 56, 24, 601, DateTimeKind.Local).AddTicks(5811), null, "Recipe3", 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

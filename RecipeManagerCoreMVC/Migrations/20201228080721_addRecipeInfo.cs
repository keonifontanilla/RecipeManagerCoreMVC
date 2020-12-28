using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeManagerCoreMVC.Migrations
{
    public partial class addRecipeInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeInfoModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrepTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CookTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInfoModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeInfoModel_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInfoModel_RecipeId",
                table: "RecipeInfoModel",
                column: "RecipeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeInfoModel");
        }
    }
}

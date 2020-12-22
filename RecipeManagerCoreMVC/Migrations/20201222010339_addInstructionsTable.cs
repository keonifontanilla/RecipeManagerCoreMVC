using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeManagerCoreMVC.Migrations
{
    public partial class addInstructionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ingredient",
                table: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_RecipeId",
                table: "Instructions",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instructions");

            migrationBuilder.RenameColumn(
                name: "Ingredient",
                table: "Ingredients",
                newName: "ingredient");
        }
    }
}

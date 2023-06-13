using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restauracja.Migrations
{
    /// <inheritdoc />
    public partial class AddedIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    IngredientID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "DishIngredient",
                columns: table => new
                {
                    DishesDishID = table.Column<long>(type: "bigint", nullable: false),
                    IngredientsIngredientID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredient", x => new { x.DishesDishID, x.IngredientsIngredientID });
                    table.ForeignKey(
                        name: "FK_DishIngredient_Dish_DishesDishID",
                        column: x => x.DishesDishID,
                        principalTable: "Dish",
                        principalColumn: "DishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredient_Ingredient_IngredientsIngredientID",
                        column: x => x.IngredientsIngredientID,
                        principalTable: "Ingredient",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_IngredientsIngredientID",
                table: "DishIngredient",
                column: "IngredientsIngredientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishIngredient");

            migrationBuilder.DropTable(
                name: "Ingredient");
        }
    }
}

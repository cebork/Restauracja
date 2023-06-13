using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restauracja.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Category_CategoryID1",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_CategoryID1",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "CategoryID1",
                table: "Dish");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Dish",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_CategoryID",
                table: "Dish",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Category_CategoryID",
                table: "Dish",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Category_CategoryID",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_CategoryID",
                table: "Dish");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryID",
                table: "Dish",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID1",
                table: "Dish",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_CategoryID1",
                table: "Dish",
                column: "CategoryID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Category_CategoryID1",
                table: "Dish",
                column: "CategoryID1",
                principalTable: "Category",
                principalColumn: "CategoryID");
        }
    }
}

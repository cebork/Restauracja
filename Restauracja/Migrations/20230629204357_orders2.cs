using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restauracja.Migrations
{
    /// <inheritdoc />
    public partial class orders2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderContent_OrderContentId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderContentId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderContentId1",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "OrderContentId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderContentId",
                table: "Order",
                column: "OrderContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderContent_OrderContentId",
                table: "Order",
                column: "OrderContentId",
                principalTable: "OrderContent",
                principalColumn: "OrderContentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderContent_OrderContentId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderContentId",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "OrderContentId",
                table: "Order",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderContentId1",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderContentId1",
                table: "Order",
                column: "OrderContentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderContent_OrderContentId1",
                table: "Order",
                column: "OrderContentId1",
                principalTable: "OrderContent",
                principalColumn: "OrderContentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

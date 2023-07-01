using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restauracja.Migrations
{
    /// <inheritdoc />
    public partial class orders24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderContent_OrderContentId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderContentId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderContent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderContent_OrderId",
                table: "OrderContent",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderContent_Order_OrderId",
                table: "OrderContent",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderContent_Order_OrderId",
                table: "OrderContent");

            migrationBuilder.DropIndex(
                name: "IX_OrderContent_OrderId",
                table: "OrderContent");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderContent");

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
    }
}

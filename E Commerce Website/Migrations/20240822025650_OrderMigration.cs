using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Website.Migrations
{
    /// <inheritdoc />
    public partial class OrderMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "customer_password",
                table: "tbl_customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cart_customer_id",
                table: "tbl_cart",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cart_product_id",
                table: "tbl_cart",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cart_tbl_customer_customer_id",
                table: "tbl_cart",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cart_tbl_product_product_id",
                table: "tbl_cart",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cart_tbl_customer_customer_id",
                table: "tbl_cart");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cart_tbl_product_product_id",
                table: "tbl_cart");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cart_customer_id",
                table: "tbl_cart");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cart_product_id",
                table: "tbl_cart");

            migrationBuilder.AlterColumn<string>(
                name: "customer_password",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}

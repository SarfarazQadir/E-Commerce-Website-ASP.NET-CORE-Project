using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Website.Migrations
{
    /// <inheritdoc />
    public partial class updatedcategoryandproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_category_id",
                table: "tbl_product",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_category_category_id",
                table: "tbl_product",
                column: "category_id",
                principalTable: "tbl_category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_category_category_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_category_id",
                table: "tbl_product");
        }
    }
}

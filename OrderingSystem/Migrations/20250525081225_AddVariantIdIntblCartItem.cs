using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantIdIntblCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tblCartItems",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "tblCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblCartItems_ProductVariantId",
                table: "tblCartItems",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCartItems_tblProductVariant_ProductVariantId",
                table: "tblCartItems",
                column: "ProductVariantId",
                principalTable: "tblProductVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCartItems_tblProductVariant_ProductVariantId",
                table: "tblCartItems");

            migrationBuilder.DropIndex(
                name: "IX_tblCartItems_ProductVariantId",
                table: "tblCartItems");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "tblCartItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tblCartItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");
        }
    }
}

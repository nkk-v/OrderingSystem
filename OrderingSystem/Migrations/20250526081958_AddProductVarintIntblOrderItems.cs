using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVarintIntblOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "tblOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItems_ProductVariantId",
                table: "tblOrderItems",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProductVariant_ProductVariantId",
                table: "tblOrderItems",
                column: "ProductVariantId",
                principalTable: "tblProductVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProductVariant_ProductVariantId",
                table: "tblOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_tblOrderItems_ProductVariantId",
                table: "tblOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "tblOrderItems");
        }
    }
}

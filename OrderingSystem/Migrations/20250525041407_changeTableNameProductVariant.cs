using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Migrations
{
    /// <inheritdoc />
    public partial class changeTableNameProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_tblProducts_ProductId",
                table: "ProductVariant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariant",
                table: "ProductVariant");

            migrationBuilder.RenameTable(
                name: "ProductVariant",
                newName: "tblProductVariant");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariant_ProductId",
                table: "tblProductVariant",
                newName: "IX_tblProductVariant_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblProductVariant",
                table: "tblProductVariant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductVariant_tblProducts_ProductId",
                table: "tblProductVariant",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductVariant_tblProducts_ProductId",
                table: "tblProductVariant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblProductVariant",
                table: "tblProductVariant");

            migrationBuilder.RenameTable(
                name: "tblProductVariant",
                newName: "ProductVariant");

            migrationBuilder.RenameIndex(
                name: "IX_tblProductVariant_ProductId",
                table: "ProductVariant",
                newName: "IX_ProductVariant_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariant",
                table: "ProductVariant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_tblProducts_ProductId",
                table: "ProductVariant",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

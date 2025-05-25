using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Migrations
{
    /// <inheritdoc />
    public partial class removePriceIntblProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "tblProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "tblCartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "tblCartItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "tblProducts",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

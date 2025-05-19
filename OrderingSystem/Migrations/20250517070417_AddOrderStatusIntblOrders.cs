using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusIntblOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tblOrders",
                newName: "DeliveryStatus");

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "tblOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "tblOrders");

            migrationBuilder.RenameColumn(
                name: "DeliveryStatus",
                table: "tblOrders",
                newName: "Status");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDelivered",
                table: "OrderDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfPayment",
                table: "OrderDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDelivered",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DateOfPayment",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderDetails");
        }
    }
}

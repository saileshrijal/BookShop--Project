using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations
{
    /// <inheritdoc />
    public partial class BookImageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeaturedImage",
                table: "Books",
                newName: "FeaturedImagePath");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BookImages",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BookImages");

            migrationBuilder.RenameColumn(
                name: "FeaturedImagePath",
                table: "Books",
                newName: "FeaturedImage");
        }
    }
}

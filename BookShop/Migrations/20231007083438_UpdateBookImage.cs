using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "BookImages",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "Alt",
                table: "BookImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "BookImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alt",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookImages");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "BookImages",
                newName: "FileName");
        }
    }
}

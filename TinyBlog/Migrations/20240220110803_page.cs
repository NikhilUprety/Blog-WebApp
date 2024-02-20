using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyBlog.Migrations
{
    public partial class page : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PageTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "PageTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PageTable");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "PageTable");
        }
    }
}

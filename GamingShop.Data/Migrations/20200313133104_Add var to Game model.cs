using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingShop.Data.Migrations
{
    public partial class AddvartoGamemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Games");
        }
    }
}

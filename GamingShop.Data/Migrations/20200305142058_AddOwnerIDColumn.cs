using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingShop.Data.Migrations
{
    public partial class AddOwnerIDColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Games");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingShop.Data.Migrations
{
    public partial class addsenderemailvariabletoMessagemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "Messages");
        }
    }
}

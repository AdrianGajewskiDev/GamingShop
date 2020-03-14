using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingShop.Data.Migrations
{
    public partial class addpostedvartoGameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Posted",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posted",
                table: "Games");
        }
    }
}

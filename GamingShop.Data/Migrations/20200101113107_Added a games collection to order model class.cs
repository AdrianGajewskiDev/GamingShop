using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingShop.Data.Migrations
{
    public partial class Addedagamescollectiontoordermodelclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_OrderID",
                table: "Games",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Orders_OrderID",
                table: "Games",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Orders_OrderID",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_OrderID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Games");
        }
    }
}

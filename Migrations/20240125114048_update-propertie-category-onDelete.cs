using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockWebApi.Migrations
{
    public partial class updatepropertiecategoryonDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marchandises_Categories_CategoryId",
                table: "Marchandises");

            migrationBuilder.AddForeignKey(
                name: "FK_Marchandises_Categories_CategoryId",
                table: "Marchandises",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marchandises_Categories_CategoryId",
                table: "Marchandises");

            migrationBuilder.AddForeignKey(
                name: "FK_Marchandises_Categories_CategoryId",
                table: "Marchandises",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

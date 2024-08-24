using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ontap_Net104_319.Migrations
{
    public partial class ok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Carts_Username",
                table: "Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Accounts_Username",
                table: "Carts",
                column: "Username",
                principalTable: "Accounts",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Accounts_Username",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Carts_Username",
                table: "Accounts",
                column: "Username",
                principalTable: "Carts",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

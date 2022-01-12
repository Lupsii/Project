using Microsoft.EntityFrameworkCore.Migrations;

namespace Lucca.DAL.Migrations
{
    public partial class addcurrencytoUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Currency",
                value: "RUB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Currency",
                value: "USD");
        }
    }
}

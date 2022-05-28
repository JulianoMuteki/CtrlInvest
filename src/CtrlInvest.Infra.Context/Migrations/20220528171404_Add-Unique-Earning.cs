using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlInvest.Infra.Context.Migrations
{
    public partial class AddUniqueEarning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Earnings_Id_DateWith",
                table: "Earnings",
                columns: new[] { "Id", "DateWith" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Earnings_Id_DateWith",
                table: "Earnings");
        }
    }
}

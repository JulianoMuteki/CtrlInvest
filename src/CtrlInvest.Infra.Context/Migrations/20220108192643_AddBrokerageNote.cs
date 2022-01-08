using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlInvest.Infra.Context.Migrations
{
    public partial class AddBrokerageNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerageNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateIssue = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    TotalAmount = table.Column<double>(type: "double precision", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TicketCode = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TypeMarket = table.Column<int>(type: "integer", nullable: false),
                    TypeDeal = table.Column<int>(type: "integer", nullable: false),
                    FinancialInstitutionID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerageNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerageNotes_Banks_FinancialInstitutionID",
                        column: x => x.FinancialInstitutionID,
                        principalTable: "Banks",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerageNotes_FinancialInstitutionID",
                table: "BrokerageNotes",
                column: "FinancialInstitutionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerageNotes");
        }
    }
}

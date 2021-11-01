using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlInvest.Infra.Context.Migrations
{
    public partial class AddTicketSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketSyncs",
                columns: table => new
                {
                    TicketSyncID = table.Column<Guid>(type: "uuid", nullable: false),
                    TickerID = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSyncs", x => new { x.TickerID, x.TicketSyncID });
                    table.ForeignKey(
                        name: "FK_TicketSyncs_Tickets_TickerID",
                        column: x => x.TickerID,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketSyncs");
        }
    }
}

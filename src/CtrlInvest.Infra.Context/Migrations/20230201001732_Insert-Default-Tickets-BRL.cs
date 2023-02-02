using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

#nullable disable

namespace CtrlInvest.Infra.Context.Migrations
{
    public partial class InsertDefaultTicketsBRL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = Path.Combine(Directory.GetCurrentDirectory(), @"Migrations/20230201001732_Insert-Default-Tickets-BRL.sql");
            migrationBuilder.Sql(File.ReadAllText(sql));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlInvest.Infra.Context.Migrations
{
    public partial class AddUserTokenHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersTokensHistories",
                columns: table => new
                {
                    UserTokenHistoryID = table.Column<Guid>(type: "uuid", nullable: false),
                    JwtId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevorked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTokensHistories", x => x.UserTokenHistoryID);
                    table.ForeignKey(
                        name: "FK_UsersTokensHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersTokensHistories_UserId",
                table: "UsersTokensHistories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersTokensHistories");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Funda.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GlobalId = table.Column<long>(nullable: false),
                    Koopprijs = table.Column<double>(nullable: true),
                    MakelaarId = table.Column<long>(nullable: false),
                    MakelaarNaam = table.Column<string>(maxLength: 128, nullable: false),
                    Woonplaats = table.Column<string>(maxLength: 128, nullable: false),
                    PublicatieDatum = table.Column<DateTime>(nullable: true),
                    HasTuin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Houses");
        }
    }
}

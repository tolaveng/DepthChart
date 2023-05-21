using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Charts",
                keyColumn: "Id",
                keyValue: new Guid("0323dab0-4be8-4db0-a28e-79f41138c3ea"));

            migrationBuilder.InsertData(
                table: "Charts",
                columns: new[] { "Id", "Depth", "Group", "PlayerNumber", "PositionId" },
                values: new object[] { new Guid("7397c214-805c-4abc-b07c-844a500e8c23"), 0, "Offense", 1, "OLB" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Charts",
                keyColumn: "Id",
                keyValue: new Guid("7397c214-805c-4abc-b07c-844a500e8c23"));

            migrationBuilder.InsertData(
                table: "Charts",
                columns: new[] { "Id", "Depth", "Group", "PlayerNumber", "PositionId" },
                values: new object[] { new Guid("0323dab0-4be8-4db0-a28e-79f41138c3ea"), 0, "Offense", 1, "OLB" });
        }
    }
}

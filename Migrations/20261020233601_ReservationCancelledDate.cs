using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreekRiver.Migrations
{
    /// <inheritdoc />
    public partial class ReservationCancelledDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "Reservations",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[] { 1, "example@email.com", "Baron", "Cloud" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Reservations");
        }
    }
}

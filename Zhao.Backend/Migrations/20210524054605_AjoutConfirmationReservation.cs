using Microsoft.EntityFrameworkCore.Migrations;

namespace Zhao.Backend.Migrations
{
    public partial class AjoutConfirmationReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Confirmation",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmation",
                table: "Reservations");
        }
    }
}

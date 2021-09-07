using Microsoft.EntityFrameworkCore.Migrations;

namespace Zhao.Backend.Migrations
{
    public partial class AjoutCourrielHtmlReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourrielHtml",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourrielHtml",
                table: "Reservations");
        }
    }
}

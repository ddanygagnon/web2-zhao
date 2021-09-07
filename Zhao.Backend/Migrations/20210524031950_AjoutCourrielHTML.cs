using Microsoft.EntityFrameworkCore.Migrations;

namespace Zhao.Backend.Migrations
{
    public partial class AjoutCourrielHTML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourrielHtml",
                table: "Evaluations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourrielHtml",
                table: "Evaluations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PanamaPrintApp.Migrations
{
    public partial class ModelFieldDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "Prices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Prices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

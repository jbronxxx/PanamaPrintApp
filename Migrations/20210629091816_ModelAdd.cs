using Microsoft.EntityFrameworkCore.Migrations;

namespace PanamaPrintApp.Migrations
{
    public partial class ModelAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelID",
                table: "Prices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ModelID",
                table: "Prices",
                column: "ModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Models_ModelID",
                table: "Prices",
                column: "ModelID",
                principalTable: "Models",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Models_ModelID",
                table: "Prices");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ModelID",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "ModelID",
                table: "Prices");
        }
    }
}

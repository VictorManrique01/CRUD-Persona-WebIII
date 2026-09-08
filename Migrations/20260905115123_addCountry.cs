using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloWorld.Migrations
{
    /// <inheritdoc />
    public partial class addCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Personas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_PaisId",
                table: "Personas",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Pais_PaisId",
                table: "Personas",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Pais_PaisId",
                table: "Personas");

            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropIndex(
                name: "IX_Personas_PaisId",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Personas");
        }
    }
}

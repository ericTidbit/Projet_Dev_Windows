using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEEEReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLibrairie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Librairie_LibrairieId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Librairie_LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Librairie");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Livres_LibrairieId",
                table: "Livres");

            migrationBuilder.DropColumn(
                name: "LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "LibrairieId",
                table: "Livres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibrairieId",
                table: "Utilisateurs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LibrairieId",
                table: "Livres",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Librairie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librairie", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_LibrairieId",
                table: "Utilisateurs",
                column: "LibrairieId");

            migrationBuilder.CreateIndex(
                name: "IX_Livres_LibrairieId",
                table: "Livres",
                column: "LibrairieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Librairie_LibrairieId",
                table: "Livres",
                column: "LibrairieId",
                principalTable: "Librairie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Librairie_LibrairieId",
                table: "Utilisateurs",
                column: "LibrairieId",
                principalTable: "Librairie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEEEReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class enleverLibraire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Librairies_LibrairieId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Librairies_LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Librairies",
                table: "Librairies");

            migrationBuilder.RenameTable(
                name: "Librairies",
                newName: "Librairie");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Librairie",
                table: "Librairie",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Librairie_LibrairieId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Librairie_LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Librairie",
                table: "Librairie");

            migrationBuilder.RenameTable(
                name: "Librairie",
                newName: "Librairies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Librairies",
                table: "Librairies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Librairies_LibrairieId",
                table: "Livres",
                column: "LibrairieId",
                principalTable: "Librairies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Librairies_LibrairieId",
                table: "Utilisateurs",
                column: "LibrairieId",
                principalTable: "Librairies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

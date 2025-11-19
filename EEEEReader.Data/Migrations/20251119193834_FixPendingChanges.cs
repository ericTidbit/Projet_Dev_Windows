using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEEEReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Librairie_LibrairieId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Clients_UtilisateurId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Librairie_LibrairieId",
                table: "Livres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Librairie",
                table: "Librairie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Librairie",
                newName: "Librairies");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Utilisateurs");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_LibrairieId",
                table: "Utilisateurs",
                newName: "IX_Utilisateurs_LibrairieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Librairies",
                table: "Librairies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utilisateurs",
                table: "Utilisateurs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Librairies_LibrairieId",
                table: "Livres",
                column: "LibrairieId",
                principalTable: "Librairies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Utilisateurs_UtilisateurId",
                table: "Livres",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Librairies_LibrairieId",
                table: "Utilisateurs",
                column: "LibrairieId",
                principalTable: "Librairies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Librairies_LibrairieId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Utilisateurs_UtilisateurId",
                table: "Livres");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Librairies_LibrairieId",
                table: "Utilisateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utilisateurs",
                table: "Utilisateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Librairies",
                table: "Librairies");

            migrationBuilder.RenameTable(
                name: "Utilisateurs",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "Librairies",
                newName: "Librairie");

            migrationBuilder.RenameIndex(
                name: "IX_Utilisateurs_LibrairieId",
                table: "Clients",
                newName: "IX_Clients_LibrairieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Librairie",
                table: "Librairie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Librairie_LibrairieId",
                table: "Clients",
                column: "LibrairieId",
                principalTable: "Librairie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Clients_UtilisateurId",
                table: "Livres",
                column: "UtilisateurId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Librairie_LibrairieId",
                table: "Livres",
                column: "LibrairieId",
                principalTable: "Librairie",
                principalColumn: "Id");
        }
    }
}

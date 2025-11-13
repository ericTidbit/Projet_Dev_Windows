using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEEEReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInitiale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Pwd = table.Column<string>(type: "TEXT", nullable: false),
                    LibrairieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Librairie_LibrairieId",
                        column: x => x.LibrairieId,
                        principalTable: "Librairie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Auteur = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    ISBN = table.Column<string>(type: "TEXT", nullable: true),
                    Langue = table.Column<string>(type: "TEXT", nullable: true),
                    Resume = table.Column<string>(type: "TEXT", nullable: true),
                    CoverRaw = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CurrentPage = table.Column<int>(type: "INTEGER", nullable: false),
                    Pourcentage = table.Column<int>(type: "INTEGER", nullable: false),
                    LibrairieId = table.Column<int>(type: "INTEGER", nullable: true),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livres_Clients_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Livres_Librairie_LibrairieId",
                        column: x => x.LibrairieId,
                        principalTable: "Librairie",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_LibrairieId",
                table: "Clients",
                column: "LibrairieId");

            migrationBuilder.CreateIndex(
                name: "IX_Livres_LibrairieId",
                table: "Livres",
                column: "LibrairieId");

            migrationBuilder.CreateIndex(
                name: "IX_Livres_UtilisateurId",
                table: "Livres",
                column: "UtilisateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livres");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Librairie");
        }
    }
}

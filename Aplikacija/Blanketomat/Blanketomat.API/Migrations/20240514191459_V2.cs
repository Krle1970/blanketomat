using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blanketomat.API.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistenti_Smerovi_SmerId",
                table: "Asistenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesori_Smerovi_SmerId",
                table: "Profesori");

            migrationBuilder.RenameColumn(
                name: "SmerId",
                table: "Profesori",
                newName: "KatedraId");

            migrationBuilder.RenameIndex(
                name: "IX_Profesori_SmerId",
                table: "Profesori",
                newName: "IX_Profesori_KatedraId");

            migrationBuilder.RenameColumn(
                name: "SmerId",
                table: "Asistenti",
                newName: "KatedraId");

            migrationBuilder.RenameIndex(
                name: "IX_Asistenti_SmerId",
                table: "Asistenti",
                newName: "IX_Asistenti_KatedraId");

            migrationBuilder.AddColumn<int>(
                name: "KatedraId",
                table: "Smerovi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KatedraId",
                table: "Predmeti",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PredmetId",
                table: "Blanketi",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AsistentSmer",
                columns: table => new
                {
                    AsistentiId = table.Column<int>(type: "int", nullable: false),
                    SmeroviId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistentSmer", x => new { x.AsistentiId, x.SmeroviId });
                    table.ForeignKey(
                        name: "FK_AsistentSmer_Asistenti_AsistentiId",
                        column: x => x.AsistentiId,
                        principalTable: "Asistenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistentSmer_Smerovi_SmeroviId",
                        column: x => x.SmeroviId,
                        principalTable: "Smerovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Katedre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Katedre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfesorSmer",
                columns: table => new
                {
                    ProfesoriId = table.Column<int>(type: "int", nullable: false),
                    SmeroviId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfesorSmer", x => new { x.ProfesoriId, x.SmeroviId });
                    table.ForeignKey(
                        name: "FK_ProfesorSmer_Profesori_ProfesoriId",
                        column: x => x.ProfesoriId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfesorSmer_Smerovi_SmeroviId",
                        column: x => x.SmeroviId,
                        principalTable: "Smerovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Smerovi_KatedraId",
                table: "Smerovi",
                column: "KatedraId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmeti_KatedraId",
                table: "Predmeti",
                column: "KatedraId");

            migrationBuilder.CreateIndex(
                name: "IX_Blanketi_PredmetId",
                table: "Blanketi",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentSmer_SmeroviId",
                table: "AsistentSmer",
                column: "SmeroviId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorSmer_SmeroviId",
                table: "ProfesorSmer",
                column: "SmeroviId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistenti_Katedre_KatedraId",
                table: "Asistenti",
                column: "KatedraId",
                principalTable: "Katedre",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blanketi_Predmeti_PredmetId",
                table: "Blanketi",
                column: "PredmetId",
                principalTable: "Predmeti",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Predmeti_Katedre_KatedraId",
                table: "Predmeti",
                column: "KatedraId",
                principalTable: "Katedre",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profesori_Katedre_KatedraId",
                table: "Profesori",
                column: "KatedraId",
                principalTable: "Katedre",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smerovi_Katedre_KatedraId",
                table: "Smerovi",
                column: "KatedraId",
                principalTable: "Katedre",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistenti_Katedre_KatedraId",
                table: "Asistenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Blanketi_Predmeti_PredmetId",
                table: "Blanketi");

            migrationBuilder.DropForeignKey(
                name: "FK_Predmeti_Katedre_KatedraId",
                table: "Predmeti");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesori_Katedre_KatedraId",
                table: "Profesori");

            migrationBuilder.DropForeignKey(
                name: "FK_Smerovi_Katedre_KatedraId",
                table: "Smerovi");

            migrationBuilder.DropTable(
                name: "AsistentSmer");

            migrationBuilder.DropTable(
                name: "Katedre");

            migrationBuilder.DropTable(
                name: "ProfesorSmer");

            migrationBuilder.DropIndex(
                name: "IX_Smerovi_KatedraId",
                table: "Smerovi");

            migrationBuilder.DropIndex(
                name: "IX_Predmeti_KatedraId",
                table: "Predmeti");

            migrationBuilder.DropIndex(
                name: "IX_Blanketi_PredmetId",
                table: "Blanketi");

            migrationBuilder.DropColumn(
                name: "KatedraId",
                table: "Smerovi");

            migrationBuilder.DropColumn(
                name: "KatedraId",
                table: "Predmeti");

            migrationBuilder.DropColumn(
                name: "PredmetId",
                table: "Blanketi");

            migrationBuilder.RenameColumn(
                name: "KatedraId",
                table: "Profesori",
                newName: "SmerId");

            migrationBuilder.RenameIndex(
                name: "IX_Profesori_KatedraId",
                table: "Profesori",
                newName: "IX_Profesori_SmerId");

            migrationBuilder.RenameColumn(
                name: "KatedraId",
                table: "Asistenti",
                newName: "SmerId");

            migrationBuilder.RenameIndex(
                name: "IX_Asistenti_KatedraId",
                table: "Asistenti",
                newName: "IX_Asistenti_SmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistenti_Smerovi_SmerId",
                table: "Asistenti",
                column: "SmerId",
                principalTable: "Smerovi",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profesori_Smerovi_SmerId",
                table: "Profesori",
                column: "SmerId",
                principalTable: "Smerovi",
                principalColumn: "Id");
        }
    }
}

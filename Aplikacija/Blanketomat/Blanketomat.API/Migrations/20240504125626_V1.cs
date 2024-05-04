using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blanketomat.API.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administratori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administratori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Akreditacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Akreditacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IspitniRokovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IspitniRokovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oblasti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oblasti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Podoblasti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podoblasti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PutanjaDoSlike = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Smerovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smerovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ponavljanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    IspitniRokId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponavljanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ponavljanja_IspitniRokovi_IspitniRokId",
                        column: x => x.IspitniRokId,
                        principalTable: "IspitniRokovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pitanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlikaId = table.Column<int>(type: "int", nullable: true),
                    OblastId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pitanja_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pitanja_Slike_SlikaId",
                        column: x => x.SlikaId,
                        principalTable: "Slike",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zadaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlikaId = table.Column<int>(type: "int", nullable: true),
                    OblastId = table.Column<int>(type: "int", nullable: true),
                    PodoblastId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zadaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zadaci_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zadaci_Podoblasti_PodoblastId",
                        column: x => x.PodoblastId,
                        principalTable: "Podoblasti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zadaci_Slike_SlikaId",
                        column: x => x.SlikaId,
                        principalTable: "Slike",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Asistenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SmerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistenti_Smerovi_SmerId",
                        column: x => x.SmerId,
                        principalTable: "Smerovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Predmeti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Godina = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AkreditacijaId = table.Column<int>(type: "int", nullable: true),
                    SmerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmeti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predmeti_Akreditacije_AkreditacijaId",
                        column: x => x.AkreditacijaId,
                        principalTable: "Akreditacije",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Predmeti_Smerovi_SmerId",
                        column: x => x.SmerId,
                        principalTable: "Smerovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profesori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SmerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profesori_Smerovi_SmerId",
                        column: x => x.SmerId,
                        principalTable: "Smerovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AkreditacijaId = table.Column<int>(type: "int", nullable: true),
                    SmerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_Akreditacije_AkreditacijaId",
                        column: x => x.AkreditacijaId,
                        principalTable: "Akreditacije",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Studenti_Smerovi_SmerId",
                        column: x => x.SmerId,
                        principalTable: "Smerovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blanketi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tip = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IspitniRokId = table.Column<int>(type: "int", nullable: true),
                    OblastId = table.Column<int>(type: "int", nullable: true),
                    PodoblastId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blanketi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blanketi_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Blanketi_Podoblasti_PodoblastId",
                        column: x => x.PodoblastId,
                        principalTable: "Podoblasti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Blanketi_Ponavljanja_IspitniRokId",
                        column: x => x.IspitniRokId,
                        principalTable: "Ponavljanja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AsistentPredmet",
                columns: table => new
                {
                    AsistentiId = table.Column<int>(type: "int", nullable: false),
                    PredmetiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistentPredmet", x => new { x.AsistentiId, x.PredmetiId });
                    table.ForeignKey(
                        name: "FK_AsistentPredmet_Asistenti_AsistentiId",
                        column: x => x.AsistentiId,
                        principalTable: "Asistenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistentPredmet_Predmeti_PredmetiId",
                        column: x => x.PredmetiId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredmetProfesor",
                columns: table => new
                {
                    PredmetiId = table.Column<int>(type: "int", nullable: false),
                    ProfesoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetProfesor", x => new { x.PredmetiId, x.ProfesoriId });
                    table.ForeignKey(
                        name: "FK_PredmetProfesor_Predmeti_PredmetiId",
                        column: x => x.PredmetiId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredmetProfesor_Profesori_ProfesoriId",
                        column: x => x.ProfesoriId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredmetStudent",
                columns: table => new
                {
                    PredmetiId = table.Column<int>(type: "int", nullable: false),
                    StudentiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetStudent", x => new { x.PredmetiId, x.StudentiId });
                    table.ForeignKey(
                        name: "FK_PredmetStudent_Predmeti_PredmetiId",
                        column: x => x.PredmetiId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredmetStudent_Studenti_StudentiId",
                        column: x => x.StudentiId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlanketPitanje",
                columns: table => new
                {
                    BlanketiId = table.Column<int>(type: "int", nullable: false),
                    PitanjaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlanketPitanje", x => new { x.BlanketiId, x.PitanjaId });
                    table.ForeignKey(
                        name: "FK_BlanketPitanje_Blanketi_BlanketiId",
                        column: x => x.BlanketiId,
                        principalTable: "Blanketi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlanketPitanje_Pitanja_PitanjaId",
                        column: x => x.PitanjaId,
                        principalTable: "Pitanja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlanketZadatak",
                columns: table => new
                {
                    BlanketiId = table.Column<int>(type: "int", nullable: false),
                    ZadaciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlanketZadatak", x => new { x.BlanketiId, x.ZadaciId });
                    table.ForeignKey(
                        name: "FK_BlanketZadatak_Blanketi_BlanketiId",
                        column: x => x.BlanketiId,
                        principalTable: "Blanketi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlanketZadatak_Zadaci_ZadaciId",
                        column: x => x.ZadaciId,
                        principalTable: "Zadaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistenti_SmerId",
                table: "Asistenti",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentPredmet_PredmetiId",
                table: "AsistentPredmet",
                column: "PredmetiId");

            migrationBuilder.CreateIndex(
                name: "IX_Blanketi_IspitniRokId",
                table: "Blanketi",
                column: "IspitniRokId");

            migrationBuilder.CreateIndex(
                name: "IX_Blanketi_OblastId",
                table: "Blanketi",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Blanketi_PodoblastId",
                table: "Blanketi",
                column: "PodoblastId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketPitanje_PitanjaId",
                table: "BlanketPitanje",
                column: "PitanjaId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketZadatak_ZadaciId",
                table: "BlanketZadatak",
                column: "ZadaciId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_OblastId",
                table: "Pitanja",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_SlikaId",
                table: "Pitanja",
                column: "SlikaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponavljanja_IspitniRokId",
                table: "Ponavljanja",
                column: "IspitniRokId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmeti_AkreditacijaId",
                table: "Predmeti",
                column: "AkreditacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmeti_SmerId",
                table: "Predmeti",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PredmetProfesor_ProfesoriId",
                table: "PredmetProfesor",
                column: "ProfesoriId");

            migrationBuilder.CreateIndex(
                name: "IX_PredmetStudent_StudentiId",
                table: "PredmetStudent",
                column: "StudentiId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesori_SmerId",
                table: "Profesori",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_AkreditacijaId",
                table: "Studenti",
                column: "AkreditacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_SmerId",
                table: "Studenti",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Zadaci_OblastId",
                table: "Zadaci",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Zadaci_PodoblastId",
                table: "Zadaci",
                column: "PodoblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Zadaci_SlikaId",
                table: "Zadaci",
                column: "SlikaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administratori");

            migrationBuilder.DropTable(
                name: "AsistentPredmet");

            migrationBuilder.DropTable(
                name: "BlanketPitanje");

            migrationBuilder.DropTable(
                name: "BlanketZadatak");

            migrationBuilder.DropTable(
                name: "PredmetProfesor");

            migrationBuilder.DropTable(
                name: "PredmetStudent");

            migrationBuilder.DropTable(
                name: "Asistenti");

            migrationBuilder.DropTable(
                name: "Pitanja");

            migrationBuilder.DropTable(
                name: "Blanketi");

            migrationBuilder.DropTable(
                name: "Zadaci");

            migrationBuilder.DropTable(
                name: "Profesori");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Ponavljanja");

            migrationBuilder.DropTable(
                name: "Oblasti");

            migrationBuilder.DropTable(
                name: "Podoblasti");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropTable(
                name: "Akreditacije");

            migrationBuilder.DropTable(
                name: "Smerovi");

            migrationBuilder.DropTable(
                name: "IspitniRokovi");
        }
    }
}

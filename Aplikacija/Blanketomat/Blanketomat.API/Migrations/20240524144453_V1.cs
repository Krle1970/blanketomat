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
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Naziv = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IspitniRokovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Katedre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Katedre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PonavljanjaIspitnihRokova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    IspitniRokId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonavljanjaIspitnihRokova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PonavljanjaIspitnihRokova_IspitniRokovi_IspitniRokId",
                        column: x => x.IspitniRokId,
                        principalTable: "IspitniRokovi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Asistenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KatedraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistenti_Katedre_KatedraId",
                        column: x => x.KatedraId,
                        principalTable: "Katedre",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profesori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KatedraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profesori_Katedre_KatedraId",
                        column: x => x.KatedraId,
                        principalTable: "Katedre",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Smerovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KatedraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smerovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Smerovi_Katedre_KatedraId",
                        column: x => x.KatedraId,
                        principalTable: "Katedre",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Oblasti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oblasti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oblasti_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id");
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
                name: "Pitanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Podoblasti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OblastId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podoblasti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Podoblasti_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zadaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OblastId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zadaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zadaci_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blanketi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Putanja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Blanketi_PonavljanjaIspitnihRokova_IspitniRokId",
                        column: x => x.IspitniRokId,
                        principalTable: "PonavljanjaIspitnihRokova",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Blanketi_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PodoblastZadatak",
                columns: table => new
                {
                    PodoblastId = table.Column<int>(type: "int", nullable: false),
                    ZadaciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodoblastZadatak", x => new { x.PodoblastId, x.ZadaciId });
                    table.ForeignKey(
                        name: "FK_PodoblastZadatak_Podoblasti_PodoblastId",
                        column: x => x.PodoblastId,
                        principalTable: "Podoblasti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PodoblastZadatak_Zadaci_ZadaciId",
                        column: x => x.ZadaciId,
                        principalTable: "Zadaci",
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

            migrationBuilder.CreateTable(
                name: "Komentari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Lajkovi = table.Column<int>(type: "int", nullable: false),
                    BlanketId = table.Column<int>(type: "int", nullable: true),
                    StudentPostavioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentari_Blanketi_BlanketId",
                        column: x => x.BlanketId,
                        principalTable: "Blanketi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Komentari_Studenti_StudentPostavioId",
                        column: x => x.StudentPostavioId,
                        principalTable: "Studenti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AsistentKomentar",
                columns: table => new
                {
                    AsistentiLikedId = table.Column<int>(type: "int", nullable: false),
                    LajkovaniKomentariId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistentKomentar", x => new { x.AsistentiLikedId, x.LajkovaniKomentariId });
                    table.ForeignKey(
                        name: "FK_AsistentKomentar_Asistenti_AsistentiLikedId",
                        column: x => x.AsistentiLikedId,
                        principalTable: "Asistenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistentKomentar_Komentari_LajkovaniKomentariId",
                        column: x => x.LajkovaniKomentariId,
                        principalTable: "Komentari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KomentarProfesor",
                columns: table => new
                {
                    LajkovaniKomentariId = table.Column<int>(type: "int", nullable: false),
                    ProfesoriLikedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomentarProfesor", x => new { x.LajkovaniKomentariId, x.ProfesoriLikedId });
                    table.ForeignKey(
                        name: "FK_KomentarProfesor_Komentari_LajkovaniKomentariId",
                        column: x => x.LajkovaniKomentariId,
                        principalTable: "Komentari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KomentarProfesor_Profesori_ProfesoriLikedId",
                        column: x => x.ProfesoriLikedId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odgovori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Lajkovi = table.Column<int>(type: "int", nullable: false),
                    KomentarId = table.Column<int>(type: "int", nullable: true),
                    StudentPostavioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odgovori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odgovori_Komentari_KomentarId",
                        column: x => x.KomentarId,
                        principalTable: "Komentari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Odgovori_Studenti_StudentPostavioId",
                        column: x => x.StudentPostavioId,
                        principalTable: "Studenti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AsistentOdgovor",
                columns: table => new
                {
                    AsistentiVerifikovaliId = table.Column<int>(type: "int", nullable: false),
                    LajkovaniOdgovoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistentOdgovor", x => new { x.AsistentiVerifikovaliId, x.LajkovaniOdgovoriId });
                    table.ForeignKey(
                        name: "FK_AsistentOdgovor_Asistenti_AsistentiVerifikovaliId",
                        column: x => x.AsistentiVerifikovaliId,
                        principalTable: "Asistenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistentOdgovor_Odgovori_LajkovaniOdgovoriId",
                        column: x => x.LajkovaniOdgovoriId,
                        principalTable: "Odgovori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdgovorProfesor",
                columns: table => new
                {
                    LajkovaniOdgovoriId = table.Column<int>(type: "int", nullable: false),
                    ProfesoriVerifikovaliId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdgovorProfesor", x => new { x.LajkovaniOdgovoriId, x.ProfesoriVerifikovaliId });
                    table.ForeignKey(
                        name: "FK_OdgovorProfesor_Odgovori_LajkovaniOdgovoriId",
                        column: x => x.LajkovaniOdgovoriId,
                        principalTable: "Odgovori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdgovorProfesor_Profesori_ProfesoriVerifikovaliId",
                        column: x => x.ProfesoriVerifikovaliId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Putanja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PitanjeId = table.Column<int>(type: "int", nullable: true),
                    ZadatakId = table.Column<int>(type: "int", nullable: true),
                    KomentarId = table.Column<int>(type: "int", nullable: true),
                    OdgovorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slike_Komentari_KomentarId",
                        column: x => x.KomentarId,
                        principalTable: "Komentari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Slike_Odgovori_OdgovorId",
                        column: x => x.OdgovorId,
                        principalTable: "Odgovori",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Slike_Pitanja_PitanjeId",
                        column: x => x.PitanjeId,
                        principalTable: "Pitanja",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Slike_Zadaci_ZadatakId",
                        column: x => x.ZadatakId,
                        principalTable: "Zadaci",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlanketSlika",
                columns: table => new
                {
                    BlanketiId = table.Column<int>(type: "int", nullable: false),
                    SlikeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlanketSlika", x => new { x.BlanketiId, x.SlikeId });
                    table.ForeignKey(
                        name: "FK_BlanketSlika_Blanketi_BlanketiId",
                        column: x => x.BlanketiId,
                        principalTable: "Blanketi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlanketSlika_Slike_SlikeId",
                        column: x => x.SlikeId,
                        principalTable: "Slike",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistenti_KatedraId",
                table: "Asistenti",
                column: "KatedraId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentKomentar_LajkovaniKomentariId",
                table: "AsistentKomentar",
                column: "LajkovaniKomentariId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentOdgovor_LajkovaniOdgovoriId",
                table: "AsistentOdgovor",
                column: "LajkovaniOdgovoriId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentPredmet_PredmetiId",
                table: "AsistentPredmet",
                column: "PredmetiId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentSmer_SmeroviId",
                table: "AsistentSmer",
                column: "SmeroviId");

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
                name: "IX_Blanketi_PredmetId",
                table: "Blanketi",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketPitanje_PitanjaId",
                table: "BlanketPitanje",
                column: "PitanjaId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketSlika_SlikeId",
                table: "BlanketSlika",
                column: "SlikeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketZadatak_ZadaciId",
                table: "BlanketZadatak",
                column: "ZadaciId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_BlanketId",
                table: "Komentari",
                column: "BlanketId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_StudentPostavioId",
                table: "Komentari",
                column: "StudentPostavioId");

            migrationBuilder.CreateIndex(
                name: "IX_KomentarProfesor_ProfesoriLikedId",
                table: "KomentarProfesor",
                column: "ProfesoriLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_Oblasti_PredmetId",
                table: "Oblasti",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Odgovori_KomentarId",
                table: "Odgovori",
                column: "KomentarId");

            migrationBuilder.CreateIndex(
                name: "IX_Odgovori_StudentPostavioId",
                table: "Odgovori",
                column: "StudentPostavioId");

            migrationBuilder.CreateIndex(
                name: "IX_OdgovorProfesor_ProfesoriVerifikovaliId",
                table: "OdgovorProfesor",
                column: "ProfesoriVerifikovaliId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_OblastId",
                table: "Pitanja",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Podoblasti_OblastId",
                table: "Podoblasti",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_PodoblastZadatak_ZadaciId",
                table: "PodoblastZadatak",
                column: "ZadaciId");

            migrationBuilder.CreateIndex(
                name: "IX_PonavljanjaIspitnihRokova_IspitniRokId",
                table: "PonavljanjaIspitnihRokova",
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
                name: "IX_Profesori_KatedraId",
                table: "Profesori",
                column: "KatedraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorSmer_SmeroviId",
                table: "ProfesorSmer",
                column: "SmeroviId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_KomentarId",
                table: "Slike",
                column: "KomentarId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_OdgovorId",
                table: "Slike",
                column: "OdgovorId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_PitanjeId",
                table: "Slike",
                column: "PitanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_ZadatakId",
                table: "Slike",
                column: "ZadatakId");

            migrationBuilder.CreateIndex(
                name: "IX_Smerovi_KatedraId",
                table: "Smerovi",
                column: "KatedraId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administratori");

            migrationBuilder.DropTable(
                name: "AsistentKomentar");

            migrationBuilder.DropTable(
                name: "AsistentOdgovor");

            migrationBuilder.DropTable(
                name: "AsistentPredmet");

            migrationBuilder.DropTable(
                name: "AsistentSmer");

            migrationBuilder.DropTable(
                name: "BlanketPitanje");

            migrationBuilder.DropTable(
                name: "BlanketSlika");

            migrationBuilder.DropTable(
                name: "BlanketZadatak");

            migrationBuilder.DropTable(
                name: "KomentarProfesor");

            migrationBuilder.DropTable(
                name: "OdgovorProfesor");

            migrationBuilder.DropTable(
                name: "PodoblastZadatak");

            migrationBuilder.DropTable(
                name: "PredmetProfesor");

            migrationBuilder.DropTable(
                name: "PredmetStudent");

            migrationBuilder.DropTable(
                name: "ProfesorSmer");

            migrationBuilder.DropTable(
                name: "Asistenti");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropTable(
                name: "Profesori");

            migrationBuilder.DropTable(
                name: "Odgovori");

            migrationBuilder.DropTable(
                name: "Pitanja");

            migrationBuilder.DropTable(
                name: "Zadaci");

            migrationBuilder.DropTable(
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "Blanketi");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Podoblasti");

            migrationBuilder.DropTable(
                name: "PonavljanjaIspitnihRokova");

            migrationBuilder.DropTable(
                name: "Oblasti");

            migrationBuilder.DropTable(
                name: "IspitniRokovi");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "Akreditacije");

            migrationBuilder.DropTable(
                name: "Smerovi");

            migrationBuilder.DropTable(
                name: "Katedre");
        }
    }
}

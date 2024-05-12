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
                name: "PonavljanjaIspitnihRokova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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
                name: "Pitanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Zadaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Putanja = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "Komentari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lajkovi = table.Column<int>(type: "int", nullable: false),
                    StudentPostavioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentari_Studenti_StudentPostavioId",
                        column: x => x.StudentPostavioId,
                        principalTable: "Studenti",
                        principalColumn: "Id");
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
                name: "Slike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Putanja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KomentarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slike_Komentari_KomentarId",
                        column: x => x.KomentarId,
                        principalTable: "Komentari",
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
                name: "IX_Asistenti_SmerId",
                table: "Asistenti",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistentKomentar_LajkovaniKomentariId",
                table: "AsistentKomentar",
                column: "LajkovaniKomentariId");

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
                name: "IX_BlanketSlika_SlikeId",
                table: "BlanketSlika",
                column: "SlikeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketZadatak_ZadaciId",
                table: "BlanketZadatak",
                column: "ZadaciId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_StudentPostavioId",
                table: "Komentari",
                column: "StudentPostavioId");

            migrationBuilder.CreateIndex(
                name: "IX_KomentarProfesor_ProfesoriLikedId",
                table: "KomentarProfesor",
                column: "ProfesoriLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_OblastId",
                table: "Pitanja",
                column: "OblastId");

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
                name: "IX_Profesori_SmerId",
                table: "Profesori",
                column: "SmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_KomentarId",
                table: "Slike",
                column: "KomentarId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administratori");

            migrationBuilder.DropTable(
                name: "AsistentKomentar");

            migrationBuilder.DropTable(
                name: "AsistentPredmet");

            migrationBuilder.DropTable(
                name: "BlanketPitanje");

            migrationBuilder.DropTable(
                name: "BlanketSlika");

            migrationBuilder.DropTable(
                name: "BlanketZadatak");

            migrationBuilder.DropTable(
                name: "KomentarProfesor");

            migrationBuilder.DropTable(
                name: "PredmetProfesor");

            migrationBuilder.DropTable(
                name: "PredmetStudent");

            migrationBuilder.DropTable(
                name: "Asistenti");

            migrationBuilder.DropTable(
                name: "Pitanja");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropTable(
                name: "Blanketi");

            migrationBuilder.DropTable(
                name: "Zadaci");

            migrationBuilder.DropTable(
                name: "Profesori");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "PonavljanjaIspitnihRokova");

            migrationBuilder.DropTable(
                name: "Oblasti");

            migrationBuilder.DropTable(
                name: "Podoblasti");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "IspitniRokovi");

            migrationBuilder.DropTable(
                name: "Akreditacije");

            migrationBuilder.DropTable(
                name: "Smerovi");
        }
    }
}

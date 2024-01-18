using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlfaPackalApi.Migrations
{
    /// <inheritdoc />
    public partial class CreacionBDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    DoctorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.DoctorID);
                });

            migrationBuilder.CreateTable(
                name: "ListasDeTrabajo",
                columns: table => new
                {
                    ListaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasDeTrabajo", x => x.ListaID);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Estudios",
                columns: table => new
                {
                    EstudioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    Modality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescripcionEstudio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TiempoEstudio = table.Column<int>(type: "int", nullable: false),
                    BodyPartExamined = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AccessionNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ListaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudios", x => x.EstudioID);
                    table.ForeignKey(
                        name: "FK_Estudios_Doctores_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctores",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estudios_ListasDeTrabajo_ListaID",
                        column: x => x.ListaID,
                        principalTable: "ListasDeTrabajo",
                        principalColumn: "ListaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estudios_Pacientes_PacienteID",
                        column: x => x.PatientID,
                        principalTable: "Pacientes",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SerieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudioID = table.Column<int>(type: "int", nullable: false),
                    SeriesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SeriesNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SerieID);
                    table.ForeignKey(
                        name: "FK_Series_Estudios_EstudioID",
                        column: x => x.EstudioID,
                        principalTable: "Estudios",
                        principalColumn: "EstudioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    ImagenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerieID = table.Column<int>(type: "int", nullable: false),
                    SOPInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ImageNumber = table.Column<int>(type: "int", nullable: false),
                    ImageLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.ImagenID);
                    table.ForeignKey(
                        name: "FK_Imagenes_Series_SerieID",
                        column: x => x.SerieID,
                        principalTable: "Series",
                        principalColumn: "SerieID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_DoctorID",
                table: "Estudios",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_ListaID",
                table: "Estudios",
                column: "ListaID");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_PacienteID",
                table: "Estudios",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_SerieID",
                table: "Imagenes",
                column: "SerieID");

            migrationBuilder.CreateIndex(
                name: "IX_Series_EstudioID",
                table: "Series",
                column: "EstudioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Estudios");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "ListasDeTrabajo");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}

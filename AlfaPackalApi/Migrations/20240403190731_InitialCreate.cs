using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    PACS_PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    GeneratedPatientID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientAge = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PatientSex = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PatientWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientBirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuerOfPatientID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.PACS_PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Estudios",
                columns: table => new
                {
                    PACS_EstudioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PACS_PatientID = table.Column<int>(type: "int", nullable: false),
                    GeneratedPatientID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StudyComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BodyPartExamined = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessionNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformingPhysicianName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExposureTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KVP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfFrames = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudios", x => x.PACS_EstudioID);
                    table.ForeignKey(
                        name: "FK_Estudios_Pacientes_PACS_PatientID",
                        column: x => x.PACS_PatientID,
                        principalTable: "Pacientes",
                        principalColumn: "PACS_PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    PACS_SerieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PACS_EstudioID = table.Column<int>(type: "int", nullable: true),
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SeriesNumber = table.Column<int>(type: "int", nullable: true),
                    SeriesDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modality = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BodyPartExamined = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PatientPosition = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.PACS_SerieID);
                    table.ForeignKey(
                        name: "FK_Series_Estudios_PACS_EstudioID",
                        column: x => x.PACS_EstudioID,
                        principalTable: "Estudios",
                        principalColumn: "PACS_EstudioID");
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    PACS_ImagenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PACS_SerieID = table.Column<int>(type: "int", nullable: false),
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SOPInstanceUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ImageNumber = table.Column<int>(type: "int", nullable: false),
                    ImageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotometricInterpretation = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    PixelSpacing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.PACS_ImagenID);
                    table.ForeignKey(
                        name: "FK_Imagenes_Series_PACS_SerieID",
                        column: x => x.PACS_SerieID,
                        principalTable: "Series",
                        principalColumn: "PACS_SerieID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudio_PacienteID_AccessionNumber",
                table: "Estudios",
                columns: new[] { "PACS_PatientID", "AccessionNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Estudio_PacienteID_EstudioID",
                table: "Estudios",
                columns: new[] { "PACS_PatientID", "PACS_EstudioID" });

            migrationBuilder.CreateIndex(
                name: "IX_Estudio_PacienteID_GeneratedPatientID",
                table: "Estudios",
                columns: new[] { "PACS_PatientID", "GeneratedPatientID" });

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_AccessionNumber",
                table: "Estudios",
                column: "AccessionNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_StudyDate",
                table: "Estudios",
                column: "StudyDate");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SerieID_ImageNumber",
                table: "Imagenes",
                columns: new[] { "PACS_SerieID", "ImageNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SOPInstanceUID_ImageNumber",
                table: "Imagenes",
                columns: new[] { "SOPInstanceUID", "ImageNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_GeneratedPatientID",
                table: "Pacientes",
                column: "GeneratedPatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_PatientName",
                table: "Pacientes",
                column: "PatientName");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_EstudioID_SeriesNumber",
                table: "Series",
                columns: new[] { "PACS_EstudioID", "SeriesNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesInstanceUID",
                table: "Series",
                column: "SeriesInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesInstanceUID_SeriesNumber",
                table: "Series",
                columns: new[] { "SeriesInstanceUID", "SeriesNumber" });
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
                name: "Pacientes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class WhiteListImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "Estudios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstado = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudades_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedInstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCiudad = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Institutions_Ciudades_IdCiudad",
                        column: x => x.IdCiudad,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DicomServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IP = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false),
                    AETitle = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PuertoRedLocal = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstitutionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicomServers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicomServers_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhitelistedIPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DicomServerId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRemoved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhitelistedIPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhitelistedIPs_DicomServers_DicomServerId",
                        column: x => x.DicomServerId,
                        principalTable: "DicomServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_InstitutionId",
                table: "Usuarios",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_InstitutionId",
                table: "Estudios",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_IdEstado",
                table: "Ciudades",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_DicomServers_InstitutionId",
                table: "DicomServers",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_IdCiudad",
                table: "Institutions",
                column: "IdCiudad");

            migrationBuilder.CreateIndex(
                name: "IX_WhitelistedIPs_DicomServerId",
                table: "WhitelistedIPs",
                column: "DicomServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_Institutions_InstitutionId",
                table: "Estudios",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Institutions_InstitutionId",
                table: "Usuarios",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_Institutions_InstitutionId",
                table: "Estudios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Institutions_InstitutionId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "WhitelistedIPs");

            migrationBuilder.DropTable(
                name: "DicomServers");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_InstitutionId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_InstitutionId",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "Estudios");
        }
    }
}

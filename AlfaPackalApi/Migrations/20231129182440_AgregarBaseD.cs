using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlfaPackalApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarBaseD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_ListasDeTrabajo_ListaID",
                table: "Estudios");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Series",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Series",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Genero",
                table: "Pacientes",
                type: "int",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Pacientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Pacientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "ListasDeTrabajo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "ListasDeTrabajo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Imagenes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Imagenes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Modality",
                table: "Estudios",
                type: "int",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "ListaID",
                table: "Estudios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Estudios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Estudios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Doctores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Doctores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesInstanceUID",
                table: "Series",
                column: "SeriesInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesNumber",
                table: "Series",
                column: "SeriesNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Nombre",
                table: "Pacientes",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Imagen_ImageNumber",
                table: "Imagenes",
                column: "ImageNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Imagen_SOPInstanceUID",
                table: "Imagenes",
                column: "SOPInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_AccessionNumber",
                table: "Estudios",
                column: "AccessionNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_StudyDate",
                table: "Estudios",
                column: "StudyDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_ListasDeTrabajo_ListaID",
                table: "Estudios",
                column: "ListaID",
                principalTable: "ListasDeTrabajo",
                principalColumn: "ListaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_ListasDeTrabajo_ListaID",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Serie_SeriesInstanceUID",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Serie_SeriesNumber",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_Nombre",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Imagen_ImageNumber",
                table: "Imagenes");

            migrationBuilder.DropIndex(
                name: "IX_Imagen_SOPInstanceUID",
                table: "Imagenes");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_AccessionNumber",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_StudyDate",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "ListasDeTrabajo");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "ListasDeTrabajo");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Doctores");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Doctores");

            migrationBuilder.AlterColumn<string>(
                name: "Genero",
                table: "Pacientes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Modality",
                table: "Estudios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ListaID",
                table: "Estudios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_ListasDeTrabajo_ListaID",
                table: "Estudios",
                column: "ListaID",
                principalTable: "ListasDeTrabajo",
                principalColumn: "ListaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

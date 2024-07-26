using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfImages",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFileSizeMB",
                table: "Series",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFileSizeMB",
                table: "Imagenes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFiles",
                table: "Estudios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeries",
                table: "Estudios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFileSizeMB",
                table: "Estudios",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PuertoRedLocal",
                table: "DicomServers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfImages",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "TotalFileSizeMB",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "TotalFileSizeMB",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "NumberOfFiles",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "NumberOfSeries",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "TotalFileSizeMB",
                table: "Estudios");

            migrationBuilder.AlterColumn<int>(
                name: "PuertoRedLocal",
                table: "DicomServers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

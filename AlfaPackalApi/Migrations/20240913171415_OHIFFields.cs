using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class OHIFFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PatientBirthDate",
                table: "Studies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StudyTime",
                table: "Studies",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ImageOrientationPatient",
                table: "Instances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePositionPatient",
                table: "Instances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransferSyntaxUID",
                table: "Instances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientBirthDate",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "StudyTime",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "ImageOrientationPatient",
                table: "Instances");

            migrationBuilder.DropColumn(
                name: "ImagePositionPatient",
                table: "Instances");

            migrationBuilder.DropColumn(
                name: "TransferSyntaxUID",
                table: "Instances");
        }
    }
}

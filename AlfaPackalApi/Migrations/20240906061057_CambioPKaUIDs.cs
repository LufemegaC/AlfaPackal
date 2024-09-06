using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class CambioPKaUIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instances_Series_StudyID_SeriesNumber",
                table: "Instances");

            migrationBuilder.DropForeignKey(
                name: "FK_InstancesDetails_Instances_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Studies_StudyID",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesDetails_Series_StudyID_SeriesNumber",
                table: "SeriesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudiesDetails_Studies_StudyID",
                table: "StudiesDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudiesDetails_StudyID",
                table: "StudiesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Studies",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Study_StudyInstanceUID",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_SeriesDetails_StudyID_SeriesNumber",
                table: "SeriesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Serie_SeriesInstanceUID",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_InstancesDetails_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instances",
                table: "Instances");

            migrationBuilder.DropIndex(
                name: "IX_Instance_SOPInstanceUID",
                table: "Instances");

            migrationBuilder.DropColumn(
                name: "StudyID",
                table: "StudiesDetails");

            migrationBuilder.DropColumn(
                name: "SeriesNumber",
                table: "SeriesDetails");

            migrationBuilder.DropColumn(
                name: "StudyID",
                table: "SeriesDetails");

            migrationBuilder.DropColumn(
                name: "StudyID",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "InstanceNumber",
                table: "InstancesDetails");

            migrationBuilder.DropColumn(
                name: "SeriesNumber",
                table: "InstancesDetails");

            migrationBuilder.DropColumn(
                name: "StudyID",
                table: "InstancesDetails");

            migrationBuilder.DropColumn(
                name: "StudyID",
                table: "Instances");

            migrationBuilder.DropColumn(
                name: "SeriesNumber",
                table: "Instances");

            migrationBuilder.RenameColumn(
                name: "StudyDetailsID",
                table: "StudiesDetails",
                newName: "StudyInstanceUID");

            migrationBuilder.RenameColumn(
                name: "SerieDetailsId",
                table: "SeriesDetails",
                newName: "SeriesInstanceUID");

            migrationBuilder.RenameColumn(
                name: "InstanceDetailsId",
                table: "InstancesDetails",
                newName: "SOPInstanceUID");

            migrationBuilder.AlterColumn<string>(
                name: "PatientName",
                table: "Studies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SeriesInstanceUID",
                table: "Series",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<int>(
                name: "SeriesNumber",
                table: "Series",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "StudyInstanceUID",
                table: "Series",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SOPInstanceUID",
                table: "Instances",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<int>(
                name: "InstanceNumber",
                table: "Instances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<string>(
                name: "SeriesInstanceUID",
                table: "Instances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Studies",
                table: "Studies",
                column: "StudyInstanceUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                column: "SeriesInstanceUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instances",
                table: "Instances",
                column: "SOPInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Study_PatientName",
                table: "Studies",
                column: "PatientName");

            migrationBuilder.CreateIndex(
                name: "IX_Study_StudyID",
                table: "Studies",
                column: "StudyID");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesDateTime",
                table: "Series",
                column: "SeriesDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Series_StudyInstanceUID",
                table: "Series",
                column: "StudyInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Instance_SeriesUID_InstanceNumber",
                table: "Instances",
                columns: new[] { "SeriesInstanceUID", "InstanceNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_Instances_Series_SeriesInstanceUID",
                table: "Instances",
                column: "SeriesInstanceUID",
                principalTable: "Series",
                principalColumn: "SeriesInstanceUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstancesDetails_Instances_SOPInstanceUID",
                table: "InstancesDetails",
                column: "SOPInstanceUID",
                principalTable: "Instances",
                principalColumn: "SOPInstanceUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Studies_StudyInstanceUID",
                table: "Series",
                column: "StudyInstanceUID",
                principalTable: "Studies",
                principalColumn: "StudyInstanceUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesDetails_Series_SeriesInstanceUID",
                table: "SeriesDetails",
                column: "SeriesInstanceUID",
                principalTable: "Series",
                principalColumn: "SeriesInstanceUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudiesDetails_Studies_StudyInstanceUID",
                table: "StudiesDetails",
                column: "StudyInstanceUID",
                principalTable: "Studies",
                principalColumn: "StudyInstanceUID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instances_Series_SeriesInstanceUID",
                table: "Instances");

            migrationBuilder.DropForeignKey(
                name: "FK_InstancesDetails_Instances_SOPInstanceUID",
                table: "InstancesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Studies_StudyInstanceUID",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesDetails_Series_SeriesInstanceUID",
                table: "SeriesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudiesDetails_Studies_StudyInstanceUID",
                table: "StudiesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Studies",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Study_PatientName",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Study_StudyID",
                table: "Studies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Serie_SeriesDateTime",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_StudyInstanceUID",
                table: "Series");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instances",
                table: "Instances");

            migrationBuilder.DropIndex(
                name: "IX_Instance_SeriesUID_InstanceNumber",
                table: "Instances");

            migrationBuilder.DropColumn(
                name: "StudyInstanceUID",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "SeriesInstanceUID",
                table: "Instances");

            migrationBuilder.RenameColumn(
                name: "StudyInstanceUID",
                table: "StudiesDetails",
                newName: "StudyDetailsID");

            migrationBuilder.RenameColumn(
                name: "SeriesInstanceUID",
                table: "SeriesDetails",
                newName: "SerieDetailsId");

            migrationBuilder.RenameColumn(
                name: "SOPInstanceUID",
                table: "InstancesDetails",
                newName: "InstanceDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "StudyID",
                table: "StudiesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PatientName",
                table: "Studies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "SeriesNumber",
                table: "SeriesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudyID",
                table: "SeriesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SeriesNumber",
                table: "Series",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "SeriesInstanceUID",
                table: "Series",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "StudyID",
                table: "Series",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "InstanceNumber",
                table: "InstancesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "SeriesNumber",
                table: "InstancesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "StudyID",
                table: "InstancesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "InstanceNumber",
                table: "Instances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "SOPInstanceUID",
                table: "Instances",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "StudyID",
                table: "Instances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "SeriesNumber",
                table: "Instances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Studies",
                table: "Studies",
                column: "StudyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                columns: new[] { "StudyID", "SeriesNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instances",
                table: "Instances",
                columns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_StudiesDetails_StudyID",
                table: "StudiesDetails",
                column: "StudyID");

            migrationBuilder.CreateIndex(
                name: "IX_Study_StudyInstanceUID",
                table: "Studies",
                column: "StudyInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesDetails_StudyID_SeriesNumber",
                table: "SeriesDetails",
                columns: new[] { "StudyID", "SeriesNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesInstanceUID",
                table: "Series",
                column: "SeriesInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_InstancesDetails_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails",
                columns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Instance_SOPInstanceUID",
                table: "Instances",
                column: "SOPInstanceUID");

            migrationBuilder.AddForeignKey(
                name: "FK_Instances_Series_StudyID_SeriesNumber",
                table: "Instances",
                columns: new[] { "StudyID", "SeriesNumber" },
                principalTable: "Series",
                principalColumns: new[] { "StudyID", "SeriesNumber" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstancesDetails_Instances_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails",
                columns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
                principalTable: "Instances",
                principalColumns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Studies_StudyID",
                table: "Series",
                column: "StudyID",
                principalTable: "Studies",
                principalColumn: "StudyID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesDetails_Series_StudyID_SeriesNumber",
                table: "SeriesDetails",
                columns: new[] { "StudyID", "SeriesNumber" },
                principalTable: "Series",
                principalColumns: new[] { "StudyID", "SeriesNumber" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudiesDetails_Studies_StudyID",
                table: "StudiesDetails",
                column: "StudyID",
                principalTable: "Studies",
                principalColumn: "StudyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class ActualizaDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstancesLoad_Instances_StudyID_SeriesNumber_ImageNumber",
                table: "InstancesLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesLoad_Series_StudyID_SeriesNumber",
                table: "SeriesLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_StudiesLoad_Studies_StudyID",
                table: "StudiesLoad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudiesLoad",
                table: "StudiesLoad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesLoad",
                table: "SeriesLoad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstancesLoad",
                table: "InstancesLoad");

            migrationBuilder.RenameTable(
                name: "StudiesLoad",
                newName: "StudiesDetails");

            migrationBuilder.RenameTable(
                name: "SeriesLoad",
                newName: "SeriesDetails");

            migrationBuilder.RenameTable(
                name: "InstancesLoad",
                newName: "InstanceDetails");

            migrationBuilder.RenameIndex(
                name: "IX_StudiesLoad_StudyID",
                table: "StudiesDetails",
                newName: "IX_StudiesDetails_StudyID");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesLoad_StudyID_SeriesNumber",
                table: "SeriesDetails",
                newName: "IX_SeriesDetails_StudyID_SeriesNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InstancesLoad_StudyID_SeriesNumber_ImageNumber",
                table: "InstanceDetails",
                newName: "IX_InstanceDetails_StudyID_SeriesNumber_ImageNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudiesDetails",
                table: "StudiesDetails",
                column: "StudyDetailsID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesDetails",
                table: "SeriesDetails",
                column: "SerieDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstanceDetails",
                table: "InstanceDetails",
                column: "InstanceDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstanceDetails_Instances_StudyID_SeriesNumber_ImageNumber",
                table: "InstanceDetails",
                columns: new[] { "StudyID", "SeriesNumber", "ImageNumber" },
                principalTable: "Instances",
                principalColumns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstanceDetails_Instances_StudyID_SeriesNumber_ImageNumber",
                table: "InstanceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesDetails_Series_StudyID_SeriesNumber",
                table: "SeriesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudiesDetails_Studies_StudyID",
                table: "StudiesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudiesDetails",
                table: "StudiesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesDetails",
                table: "SeriesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstanceDetails",
                table: "InstanceDetails");

            migrationBuilder.RenameTable(
                name: "StudiesDetails",
                newName: "StudiesLoad");

            migrationBuilder.RenameTable(
                name: "SeriesDetails",
                newName: "SeriesLoad");

            migrationBuilder.RenameTable(
                name: "InstanceDetails",
                newName: "InstancesLoad");

            migrationBuilder.RenameIndex(
                name: "IX_StudiesDetails_StudyID",
                table: "StudiesLoad",
                newName: "IX_StudiesLoad_StudyID");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesDetails_StudyID_SeriesNumber",
                table: "SeriesLoad",
                newName: "IX_SeriesLoad_StudyID_SeriesNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InstanceDetails_StudyID_SeriesNumber_ImageNumber",
                table: "InstancesLoad",
                newName: "IX_InstancesLoad_StudyID_SeriesNumber_ImageNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudiesLoad",
                table: "StudiesLoad",
                column: "StudyDetailsID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesLoad",
                table: "SeriesLoad",
                column: "SerieDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstancesLoad",
                table: "InstancesLoad",
                column: "InstanceDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstancesLoad_Instances_StudyID_SeriesNumber_ImageNumber",
                table: "InstancesLoad",
                columns: new[] { "StudyID", "SeriesNumber", "ImageNumber" },
                principalTable: "Instances",
                principalColumns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesLoad_Series_StudyID_SeriesNumber",
                table: "SeriesLoad",
                columns: new[] { "StudyID", "SeriesNumber" },
                principalTable: "Series",
                principalColumns: new[] { "StudyID", "SeriesNumber" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudiesLoad_Studies_StudyID",
                table: "StudiesLoad",
                column: "StudyID",
                principalTable: "Studies",
                principalColumn: "StudyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

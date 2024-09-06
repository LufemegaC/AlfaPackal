using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_PACsServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstanceDetails_Instances_StudyID_SeriesNumber_ImageNumber",
                table: "InstanceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstanceDetails",
                table: "InstanceDetails");

            migrationBuilder.RenameTable(
                name: "InstanceDetails",
                newName: "InstancesDetails");

            migrationBuilder.RenameColumn(
                name: "ImageNumber",
                table: "InstancesDetails",
                newName: "InstanceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InstanceDetails_StudyID_SeriesNumber_ImageNumber",
                table: "InstancesDetails",
                newName: "IX_InstancesDetails_StudyID_SeriesNumber_InstanceNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstancesDetails",
                table: "InstancesDetails",
                column: "InstanceDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstancesDetails_Instances_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails",
                columns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
                principalTable: "Instances",
                principalColumns: new[] { "StudyID", "SeriesNumber", "InstanceNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstancesDetails_Instances_StudyID_SeriesNumber_InstanceNumber",
                table: "InstancesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstancesDetails",
                table: "InstancesDetails");

            migrationBuilder.RenameTable(
                name: "InstancesDetails",
                newName: "InstanceDetails");

            migrationBuilder.RenameColumn(
                name: "InstanceNumber",
                table: "InstanceDetails",
                newName: "ImageNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InstancesDetails_StudyID_SeriesNumber_InstanceNumber",
                table: "InstanceDetails",
                newName: "IX_InstanceDetails_StudyID_SeriesNumber_ImageNumber");

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
        }
    }
}

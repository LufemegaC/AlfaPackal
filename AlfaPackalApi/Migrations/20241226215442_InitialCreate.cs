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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalDicomServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IP = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false),
                    AETitle = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalDicomServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Studies",
                columns: table => new
                {
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudyTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferringPhysicianName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientAge = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PatientSex = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PatientWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientBirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuerOfPatientID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studies", x => x.StudyInstanceUID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateID",
                        column: x => x.StateID,
                        principalTable: "States",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeriesNumber = table.Column<int>(type: "int", nullable: false),
                    SeriesDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BodyPartExamined = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtocolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesInstanceUID);
                    table.ForeignKey(
                        name: "FK_Series_Studies_StudyInstanceUID",
                        column: x => x.StudyInstanceUID,
                        principalTable: "Studies",
                        principalColumn: "StudyInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyDetails",
                columns: table => new
                {
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfStudyRelatedInstances = table.Column<int>(type: "int", nullable: true),
                    NumberOfStudyRelatedSeries = table.Column<int>(type: "int", nullable: true),
                    AccessionNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TotalFileSizeMB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDetails", x => x.StudyInstanceUID);
                    table.ForeignKey(
                        name: "FK_StudyDetails_Studies_StudyInstanceUID",
                        column: x => x.StudyInstanceUID,
                        principalTable: "Studies",
                        principalColumn: "StudyInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyModalities",
                columns: table => new
                {
                    ModalityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Modality = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyModalities", x => x.ModalityID);
                    table.ForeignKey(
                        name: "FK_StudyModalities_Studies_StudyInstanceUID",
                        column: x => x.StudyInstanceUID,
                        principalTable: "Studies",
                        principalColumn: "StudyInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instances",
                columns: table => new
                {
                    SOPInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstanceNumber = table.Column<int>(type: "int", nullable: false),
                    SOPClassUID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TransferSyntaxUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotometricInterpretation = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    ImagePositionPatient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageOrientationPatient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PixelSpacing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BitsAllocated = table.Column<int>(type: "int", nullable: false),
                    SliceThickness = table.Column<float>(type: "real", nullable: false),
                    SliceLocation = table.Column<float>(type: "real", nullable: false),
                    InstanceCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstanceCreationTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfFrames = table.Column<int>(type: "int", nullable: true),
                    TransactionUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instances", x => x.SOPInstanceUID);
                    table.ForeignKey(
                        name: "FK_Instances_Series_SeriesInstanceUID",
                        column: x => x.SeriesInstanceUID,
                        principalTable: "Series",
                        principalColumn: "SeriesInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerieDetails",
                columns: table => new
                {
                    SeriesInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfSeriesRelatedInstances = table.Column<int>(type: "int", nullable: true),
                    TotalFileSizeMB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerieDetails", x => x.SeriesInstanceUID);
                    table.ForeignKey(
                        name: "FK_SerieDetails_Series_SeriesInstanceUID",
                        column: x => x.SeriesInstanceUID,
                        principalTable: "Series",
                        principalColumn: "SeriesInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstanceDetails",
                columns: table => new
                {
                    SOPInstanceUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFileSizeMB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstanceDetails", x => x.SOPInstanceUID);
                    table.ForeignKey(
                        name: "FK_InstanceDetails_Instances_SOPInstanceUID",
                        column: x => x.SOPInstanceUID,
                        principalTable: "Instances",
                        principalColumn: "SOPInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateID",
                table: "Cities",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_Instance_SeriesUID_InstanceNumber",
                table: "Instances",
                columns: new[] { "SeriesInstanceUID", "InstanceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Serie_SeriesDateTime",
                table: "Series",
                column: "SeriesDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Series_StudyInstanceUID",
                table: "Series",
                column: "StudyInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Study_PatientName",
                table: "Studies",
                column: "PatientName");

            migrationBuilder.CreateIndex(
                name: "IX_Study_StudyDate",
                table: "Studies",
                column: "StudyDate");

            migrationBuilder.CreateIndex(
                name: "IX_Study_StudyID",
                table: "Studies",
                column: "StudyID");

            migrationBuilder.CreateIndex(
                name: "IX_StudyModalities_StudyInstanceUID",
                table: "StudyModalities",
                column: "StudyInstanceUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "InstanceDetails");

            migrationBuilder.DropTable(
                name: "LocalDicomServers");

            migrationBuilder.DropTable(
                name: "SerieDetails");

            migrationBuilder.DropTable(
                name: "StudyDetails");

            migrationBuilder.DropTable(
                name: "StudyModalities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Instances");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Studies");
        }
    }
}

﻿// <auto-generated />
using System;
using AlfaPackalApi.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api_PACsServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241016070650_DeleteInstitution")]
    partial class DeleteInstitution
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlfaPackalApi.Modelos.Serie", b =>
                {
                    b.Property<string>("SeriesInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Modality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientPosition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SeriesDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SeriesDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeriesNumber")
                        .HasColumnType("int");

                    b.Property<string>("StudyInstanceUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SeriesInstanceUID");

                    b.HasIndex("SeriesDateTime")
                        .HasDatabaseName("IX_Serie_SeriesDateTime");

                    b.HasIndex("StudyInstanceUID");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.AccessControl.LocalDicomServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AETitle")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasMaxLength(39)
                        .HasColumnType("nvarchar(39)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LocalDicomServers");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.AccessControl.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Geography.City", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("StateID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Geography.State", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("States");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Instance", b =>
                {
                    b.Property<string>("SOPInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Columns")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageComments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageOrientationPatient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePositionPatient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstanceNumber")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfFrames")
                        .HasColumnType("int");

                    b.Property<string>("PhotometricInterpretation")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("PixelSpacing")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rows")
                        .HasColumnType("int");

                    b.Property<string>("SOPClassUID")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("SeriesInstanceUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TransferSyntaxUID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SOPInstanceUID");

                    b.HasIndex("SeriesInstanceUID", "InstanceNumber")
                        .HasDatabaseName("IX_Instance_SeriesUID_InstanceNumber");

                    b.ToTable("Instances");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.InstanceDetails", b =>
                {
                    b.Property<string>("SOPInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalFileSizeMB")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SOPInstanceUID");

                    b.ToTable("InstancesDetails");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.SerieDetails", b =>
                {
                    b.Property<string>("SeriesInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("NumberOfSeriesRelatedInstances")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalFileSizeMB")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SeriesInstanceUID");

                    b.ToTable("SeriesDetails");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.StudyDetails", b =>
                {
                    b.Property<string>("StudyInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccessionNumber")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("NumberOfStudyRelatedInstances")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfStudyRelatedSeries")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalFileSizeMB")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("StudyInstanceUID");

                    b.ToTable("StudiesDetails");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Study", b =>
                {
                    b.Property<string>("StudyInstanceUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BodyPartExamined")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InstitutionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssuerOfPatientID")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Modality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientAge")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<DateTime?>("PatientBirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PatientSex")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("PatientWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StudyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StudyDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudyID"));

                    b.Property<TimeSpan?>("StudyTime")
                        .HasColumnType("time");

                    b.HasKey("StudyInstanceUID");

                    b.HasIndex("PatientName")
                        .HasDatabaseName("IX_Study_PatientName");

                    b.HasIndex("StudyDate")
                        .HasDatabaseName("IX_Study_StudyDate");

                    b.HasIndex("StudyID")
                        .HasDatabaseName("IX_Study_StudyID");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Names")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AlfaPackalApi.Modelos.Serie", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.Study", "Study")
                        .WithMany("Series")
                        .HasForeignKey("StudyInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Study");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Geography.City", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.Geography.State", "State")
                        .WithMany()
                        .HasForeignKey("StateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Instance", b =>
                {
                    b.HasOne("AlfaPackalApi.Modelos.Serie", "Serie")
                        .WithMany("Instances")
                        .HasForeignKey("SeriesInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Serie");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.InstanceDetails", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.Instance", "Instance")
                        .WithOne("InstanceDetails")
                        .HasForeignKey("Api_PACsServer.Modelos.Load.InstanceDetails", "SOPInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instance");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.SerieDetails", b =>
                {
                    b.HasOne("AlfaPackalApi.Modelos.Serie", "Serie")
                        .WithOne("SerieDetails")
                        .HasForeignKey("Api_PACsServer.Modelos.Load.SerieDetails", "SeriesInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Serie");
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Load.StudyDetails", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.Study", "Study")
                        .WithOne("StudyDetails")
                        .HasForeignKey("Api_PACsServer.Modelos.Load.StudyDetails", "StudyInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Study");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.AccessControl.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.AccessControl.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api_PACsServer.Modelos.AccessControl.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Api_PACsServer.Modelos.AccessControl.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AlfaPackalApi.Modelos.Serie", b =>
                {
                    b.Navigation("Instances");

                    b.Navigation("SerieDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Instance", b =>
                {
                    b.Navigation("InstanceDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("Api_PACsServer.Modelos.Study", b =>
                {
                    b.Navigation("Series");

                    b.Navigation("StudyDetails")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
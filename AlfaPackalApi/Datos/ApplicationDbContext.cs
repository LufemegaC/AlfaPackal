using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Geography;
using Api_PACsServer.Modelos.Load;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Datos
{
    public class ApplicationDbContext : IdentityDbContext<SystemUser>
    {
        //DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        // User Control
        public DbSet<User> Users { get; set; } // Cambiarlo a users
        // Pacs information
        public DbSet<Study> Studies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Instance> Instances { get; set; }
        // Entidades para informacion de carga
        public DbSet<StudyDetails> StudiesDetails { get; set; }
        public DbSet<SerieDetails> SeriesDetails { get; set; }
        public DbSet<InstanceDetails> InstancesDetails { get; set; }
        // Dicom Servers information
        //public DbSet<AllowedIPs> AllowedIPs { get; set; }
        public DbSet<LocalDicomServer> LocalDicomServers { get; set; }
        // Internal control
        //public DbSet<Institution> Institutions { get; set; }
        public DbSet<State> States{ get; set; }
        public DbSet<City> Cities { get; set; }
        
        //// Modelo para visualizacion de datos
        //public DbSet<EstudioConPacienteDto> EstudiosConPacienteDtos { get; set; }

        // Tabla de identity + Institurion
        public DbSet<SystemUser> SystemUsers { get; set; }


        //Agrego indices secundarios para busquedas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuratio precition
            modelBuilder.Entity<InstanceDetails>()
                .Property(e => e.TotalFileSizeMB)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SerieDetails>()
                .Property(e => e.TotalFileSizeMB)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudyDetails>()
                .Property(e => e.TotalFileSizeMB)
                .HasColumnType("decimal(18,2)");
            // Sobreecribo metodo para agregar indices secundarios
            base.OnModelCreating(modelBuilder);

            // ** Study ** //
            modelBuilder.Entity<Study>()
                .HasKey(s => s.StudyInstanceUID); // PK basada en StudyInstanceUID

            // Índice para StudyDate
            modelBuilder.Entity<Study>()
                .HasIndex(s => s.StudyDate)
                .HasDatabaseName("IX_Study_StudyDate");

            // Índice para StudyID (campo autogenerado, pero no PK)
            modelBuilder.Entity<Study>()
                .HasIndex(s => s.StudyID)
                .HasDatabaseName("IX_Study_StudyID");

            // Índice para PatientName
            modelBuilder.Entity<Study>()
                .HasIndex(s => s.PatientName)
                .HasDatabaseName("IX_Study_PatientName");

            modelBuilder.Entity<Study>()
                .Property(s => s.StudyID)
                .ValueGeneratedOnAdd(); // Mantiene la generación automática de StudyID

            // ** Serie ** //
            modelBuilder.Entity<Serie>()
                .HasKey(s => s.SeriesInstanceUID); // PK basada en SeriesInstanceUID

            // Índice para SeriesDateTime
            modelBuilder.Entity<Serie>()
                .HasIndex(s => s.SeriesDateTime)
                .HasDatabaseName("IX_Serie_SeriesDateTime");

            modelBuilder.Entity<Serie>()
                .HasOne(s => s.Study)
                .WithMany(st => st.Series)
                .HasForeignKey(s => s.StudyInstanceUID); // FK hacia Study en StudyInstanceUID

            // ** Instance ** //
            modelBuilder.Entity<Instance>()
                .HasKey(i => i.SOPInstanceUID); // PK basada en SOPInstanceUID

            // Índice para combinación de SeriesInstanceUID y InstanceNumber
            modelBuilder.Entity<Instance>()
                .HasIndex(i => new { i.SeriesInstanceUID, i.InstanceNumber })
                .HasDatabaseName("IX_Instance_SeriesUID_InstanceNumber");

            modelBuilder.Entity<Instance>()
                .HasOne(i => i.Serie)
                .WithMany(s => s.Instances)
                .HasForeignKey(i => i.SeriesInstanceUID); // FK hacia Serie en SeriesInstanceUID

            // ** StudyDetails ** //
            modelBuilder.Entity<StudyDetails>()
                .HasKey(sd => sd.StudyInstanceUID); // PK basada en StudyInstanceUID

            modelBuilder.Entity<StudyDetails>()
                .HasOne(sd => sd.Study)
                .WithOne(s => s.StudyDetails)
                .HasForeignKey<StudyDetails>(sd => sd.StudyInstanceUID); // FK hacia Study en StudyInstanceUID

            // ** SerieDetails ** //
            modelBuilder.Entity<SerieDetails>()
                .HasKey(sd => sd.SeriesInstanceUID); // PK basada en SeriesInstanceUID

            modelBuilder.Entity<SerieDetails>()
            .HasOne(sd => sd.Serie)
            .WithOne(s => s.SerieDetails)
            .HasForeignKey<SerieDetails>(sd => sd.SeriesInstanceUID); // FK hacia Serie en SeriesInstanceUID

            // ** InstanceDetails ** //
            modelBuilder.Entity<InstanceDetails>()
                .HasKey(id => id.SOPInstanceUID); // PK basada en SOPInstanceUID

            modelBuilder.Entity<InstanceDetails>()
                .HasOne(id => id.Instance)
                .WithOne(i => i.InstanceDetails)
                .HasForeignKey<InstanceDetails>(id => id.SOPInstanceUID); // FK hacia Instance en SOPInstanceUID
        }
    }
}

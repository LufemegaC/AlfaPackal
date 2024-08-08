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
        public DbSet<StudyLoad> StudiesLoad { get; set; }
        public DbSet<SerieLoad> SeriesLoad { get; set; }
        public DbSet<InstanceLoad> InstancesLoad { get; set; }
        // Dicom Servers information
        //public DbSet<AllowedIPs> AllowedIPs { get; set; }
        public DbSet<LocalDicomServer> LocalDicomServers { get; set; }
        // Internal control
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<State> States{ get; set; }
        public DbSet<City> Cities { get; set; }
        
        //// Modelo para visualizacion de datos
        //public DbSet<EstudioConPacienteDto> EstudiosConPacienteDtos { get; set; }

        // Tabla de identity + Institurion
        public DbSet<SystemUser> SystemUsers { get; set; }


        //Agrego indices secundarios para busquedas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sobreecribo metodo para agregar indices secundarios
            base.OnModelCreating(modelBuilder);
            // * Paciente
            // Crear índice para AccessionNumber en la entidad Estudio
            // 12/01/2024 .- Luis Felipe M Indice por nombre
            // 26/07/2024 .- Luis Felipe M Se retira la entidad, se unifica con Study
            //modelBuilder.Entity<Paciente>()
            //   .HasIndex(e => new { e.PatientName })
            //   .HasDatabaseName("IX_Paciente_PatientName");
            //modelBuilder.Entity<Paciente>()
            //   .HasIndex(e => new { e.GeneratedPatientID })
            //   .HasDatabaseName("IX_Paciente_GeneratedPatientID");
            // * Estudio
            // Crear índice para StudyDate en la entidad Estudio
            modelBuilder.Entity<Study>()
                .HasIndex(e => e.StudyDate)
                .HasDatabaseName("IX_Studies_StudyDate"); // IX_Estudios_StudyDate 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Study>()
                .HasIndex(e => e.AccessionNumber)
                .HasDatabaseName("IX_Studies_AccessionNumber"); // IX_Estudios_AccessionNumber 
            // Indices de busqueda por Paciente y ID
            //modelBuilder.Entity<Study>()
            //    .HasIndex(e => new { e.PACS_PatientID, e.PACS_EstudioID })
            //    .HasDatabaseName("IX_Estudio_PacienteID_EstudioID"); // IX_Estudio_PacienteID_EstudioID
            // Indices de busqueda por Paciente y numero de acceso
            //modelBuilder.Entity<Study>()
            //    .HasIndex(e => new { e.PACS_PatientID, e.AccessionNumber })
            //    .HasDatabaseName("IX_Estudio_PacienteID_AccessionNumber"); // IX_Estudio_PacienteID_EstudioID  
            //                                                               // Indices de busqueda por Paciente y numero de acceso
            //modelBuilder.Entity<Study>()
            //    .HasIndex(e => new { e.PACS_PatientID, e.GeneratedPatientID })
            //    .HasDatabaseName("IX_Estudio_PacienteID_GeneratedPatientID");
            // * Serie *//
            modelBuilder.Entity<Serie>()
                .HasIndex(e => e.SeriesInstanceUID)
                .HasDatabaseName("IX_Serie_SeriesInstanceUID"); // IX_Estudios_AccessionNumber 
            // Crear índice compuesto para SeriesInstanceUID y SeriesNumber en la entidad Serie
            modelBuilder.Entity<Serie>()
                .HasIndex(e => new { e.SeriesInstanceUID, e.SeriesNumber })
                .HasDatabaseName("IX_Serie_SeriesInstanceUID_SeriesNumber");
            // Crear índice compuesto para Estudio y Numero
            modelBuilder.Entity<Serie>()
                .HasIndex(e => new { e.PACSStudyID, e.SeriesNumber })
                .HasDatabaseName("IX_Serie_EstudioID_SeriesNumber");
            // * Instance
            modelBuilder.Entity<Instance>()
                .HasIndex(e => new { e.SOPInstanceUID, e.ImageNumber })
                .HasDatabaseName("IX_Instance_SOPInstanceUID_ImageNumber");
            modelBuilder.Entity<Instance>()
                .HasIndex(e => new { e.PACSSerieID, e.ImageNumber })
                .HasDatabaseName("IX_Instance_SerieID_ImageNumber");

            //////--- Configuracion de para mapeo de entidad de listado de estudios con pacientes
            ////
            //modelBuilder.Entity<EstudioConPacienteDto>(eb =>
            //{
            //    eb.HasNoKey(); // Indica que no tiene una clave primaria, ya que es solo para lectura
            //    eb.ToView("sql1"); // Especifica que no está mapeado a una vista o tabla específica
            //});
        }
    }
}

using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Geografia;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlfaPackalApi.Datos
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioSistema>
    {
        //DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        //public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        //public DbSet<ListaDeTrabajo> ListasDeTrabajo { get; set; }
        public DbSet<Estudio> Estudios { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        // Lista blanca de IPs
        public DbSet<WhitelistedIP> WhitelistedIPs { get; set; }
        public DbSet<DicomServer> DicomServers { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<EstudioConPacienteDto> EstudiosConPacienteDtos { get; set; }

        // Tabla de identity + Institurion
        public DbSet<UsuarioSistema> UsuariosSistema { get; set; }


        //Agrego indices secundarios para busquedas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sobreecribo metodo para agregar indices secundarios
            base.OnModelCreating(modelBuilder);
            // * Paciente
            // Crear índice para AccessionNumber en la entidad Estudio
            // 12/01/2024 .- Luis Felipe M Indice por nombre
            modelBuilder.Entity<Paciente>()
               .HasIndex(e => new { e.PatientName })
               .HasDatabaseName("IX_Paciente_PatientName");
            modelBuilder.Entity<Paciente>()
               .HasIndex(e => new { e.GeneratedPatientID })
               .HasDatabaseName("IX_Paciente_GeneratedPatientID");
            // * Estudio
            // Crear índice para StudyDate en la entidad Estudio
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => e.StudyDate)
                .HasDatabaseName("IX_Estudios_StudyDate"); // IX_Estudios_StudyDate 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => e.AccessionNumber)
                .HasDatabaseName("IX_Estudios_AccessionNumber"); // IX_Estudios_AccessionNumber 
            // Indices de busqueda por Paciente y ID
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => new { e.PACS_PatientID, e.PACS_EstudioID })
                .HasDatabaseName("IX_Estudio_PacienteID_EstudioID"); // IX_Estudio_PacienteID_EstudioID
            // Indices de busqueda por Paciente y numero de acceso
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => new { e.PACS_PatientID, e.AccessionNumber })
                .HasDatabaseName("IX_Estudio_PacienteID_AccessionNumber"); // IX_Estudio_PacienteID_EstudioID  
                                                                           // Indices de busqueda por Paciente y numero de acceso
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => new { e.PACS_PatientID, e.GeneratedPatientID })
                .HasDatabaseName("IX_Estudio_PacienteID_GeneratedPatientID");
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
                .HasIndex(e => new { e.PACS_EstudioID, e.SeriesNumber })
                .HasDatabaseName("IX_Serie_EstudioID_SeriesNumber");
            // * Imagen
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => new { e.SOPInstanceUID, e.ImageNumber })
                .HasDatabaseName("IX_Serie_SOPInstanceUID_ImageNumber");
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => new { e.PACS_SerieID, e.ImageNumber })
                .HasDatabaseName("IX_Serie_SerieID_ImageNumber");

            ////--- Configuracion de para mapeo de entidad de listado de estudios con pacientes
            //
            modelBuilder.Entity<EstudioConPacienteDto>(eb =>
            {
                eb.HasNoKey(); // Indica que no tiene una clave primaria, ya que es solo para lectura
                eb.ToView("sql1"); // Especifica que no está mapeado a una vista o tabla específica
            });
        }

        public override int SaveChanges()
        {
            var entradas = ChangeTracker.Entries().Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entrada in entradas)
            {
                var entidad = (IAuditable)entrada.Entity;

                if (entrada.State == EntityState.Added)
                {
                    entidad.CreationDate = DateTime.Now;
                }

                entidad.UpdateDate = DateTime.Now;
            }


            return base.SaveChanges();
        }

    }
}

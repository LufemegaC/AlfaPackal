using AlfaPackalApi.Modelos;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlfaPackalApi.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<ListaDeTrabajo> ListasDeTrabajo { get; set; }
        public DbSet<Estudio> Estudios { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }

        //Agrego indices secundarios para busquedas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sobreecribo metodo para agregar indices secundarios
            base.OnModelCreating(modelBuilder);
            // Crear índice para StudyDate en la entidad Estudio
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => e.StudyDate)
                .HasDatabaseName("IX_Estudios_StudyDate"); // IX_Estudios_StudyDate 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => e.AccessionNumber)
                .HasDatabaseName("IX_Estudios_AccessionNumber"); // IX_Estudios_AccessionNumber 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => e.SOPInstanceUID)
                .HasDatabaseName("IX_Imagen_SOPInstanceUID"); // IX_Estudios_AccessionNumber 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => e.ImageNumber)
                .HasDatabaseName("IX_Imagen_ImageNumber"); // IX_Estudios_AccessionNumber 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Paciente>()
                .HasIndex(e => e.Nombre)
                .HasDatabaseName("IX_Paciente_Nombre"); // IX_Estudios_AccessionNumber
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Serie>()
                .HasIndex(e => e.SeriesInstanceUID)
                .HasDatabaseName("IX_Serie_SeriesInstanceUID"); // IX_Estudios_AccessionNumber 
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Serie>()
                .HasIndex(e => e.SeriesNumber)
                .HasDatabaseName("IX_Serie_SeriesNumber"); // IX_Estudios_AccessionNumber
    }

        public override int SaveChanges()
        {
            var entradas = ChangeTracker.Entries().Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entrada in entradas)
            {
                var entidad = (IAuditable)entrada.Entity;

                if (entrada.State == EntityState.Added)
                {
                    entidad.FechaCreacion = DateTime.Now;
                }

                entidad.FechaActualizacion = DateTime.Now;
            }

            return base.SaveChanges();
        }

    }
}

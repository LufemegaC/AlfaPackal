﻿using AlfaPackalApi.Modelos;
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
            // * Paciente
            // Crear índice para AccessionNumber en la entidad Estudio
            modelBuilder.Entity<Paciente>()
               .HasIndex(e => new { e.Nombre, e.Apellido })
               .HasDatabaseName("IX_Paciente_Nombre_Apellido");
            modelBuilder.Entity<Paciente>()
               .HasIndex(e => new { e.Apellido, e.Nombre })
               .HasDatabaseName("IX_Paciente_Apellido_Nombre");
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
                .HasIndex(e => new { e.PacienteID, e.EstudioID  })
                .HasDatabaseName("IX_Estudio_PacienteID_EstudioID"); // IX_Estudio_PacienteID_EstudioID
            // Indices de busqueda por Paciente y numero de acceso
            modelBuilder.Entity<Estudio>()
                .HasIndex(e => new { e.PacienteID, e.AccessionNumber })
                .HasDatabaseName("IX_Estudio_PacienteID_AccessionNumber"); // IX_Estudio_PacienteID_EstudioID  
            // * Serie
            modelBuilder.Entity<Serie>()
                .HasIndex(e => e.SeriesInstanceUID)
                .HasDatabaseName("IX_Serie_SeriesInstanceUID"); // IX_Estudios_AccessionNumber 
            // Crear índice compuesto para SeriesInstanceUID y SeriesNumber en la entidad Serie
            modelBuilder.Entity<Serie>()
                .HasIndex(e => new { e.SeriesInstanceUID, e.SeriesNumber })
                .HasDatabaseName("IX_Serie_SeriesInstanceUID_SeriesNumber");
            // Crear índice compuesto para Estudio y Numero
            modelBuilder.Entity<Serie>()
                .HasIndex(e => new { e.EstudioID, e.SeriesNumber })
                .HasDatabaseName("IX_Serie_EstudioID_SeriesNumber");
            // * Imagen
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => new { e.SOPInstanceUID, e.ImageNumber })
                .HasDatabaseName("IX_Serie_SOPInstanceUID_ImageNumber");
            modelBuilder.Entity<Imagen>()
                .HasIndex(e => new { e.SerieID, e.ImageNumber })
                .HasDatabaseName("IX_Serie_SerieID_ImageNumber");
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

using Microsoft.EntityFrameworkCore;
using SistemaGestionProyectos.Dominio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Infraestructura.Datos
{
    public class ProyectosDbContext : DbContext
    {
        public ProyectosDbContext(DbContextOptions<ProyectosDbContext> options) : base(options) { }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<ProyectoEquipo> ProyectosEquipos { get; set; }
        public DbSet<UsuarioEquipo> UsuariosEquipos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Proyecto>().ToTable("Proyectos");
            modelBuilder.Entity<Tarea>().ToTable("Tareas");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Equipo>().ToTable("Equipos");
            modelBuilder.Entity<ProyectoEquipo>().ToTable("Proyectos_Equipos");
            modelBuilder.Entity<UsuarioEquipo>().ToTable("Usuarios_Equipos");

            modelBuilder.Entity<Proyecto>().HasKey(p => p.IdProyecto);
            modelBuilder.Entity<Tarea>().HasKey(p => p.IdTarea);
            modelBuilder.Entity<Usuario>().HasKey(p => p.IdUsuario);
            modelBuilder.Entity<Equipo>().HasKey(p => p.IdEquipo);
            modelBuilder.Entity<ProyectoEquipo>().HasKey(p => p.IdProyectoEquipo);
            modelBuilder.Entity<UsuarioEquipo>().HasKey(p => p.IdUsuarioEquipo);

            // Configuración de relaciones
            modelBuilder.Entity<ProyectoEquipo>()
                .HasOne(pe => pe.Proyecto)
                .WithMany(p => p.ProyectoEquipos)
                .HasForeignKey(pe => pe.IdProyecto);

            modelBuilder.Entity<ProyectoEquipo>()
                .HasOne(pe => pe.Equipo)
                .WithMany(e => e.ProyectoEquipos)
                .HasForeignKey(pe => pe.IdEquipo);

            modelBuilder.Entity<UsuarioEquipo>()
                .HasOne(ue => ue.Usuario)
                .WithMany()
                .HasForeignKey(ue => ue.IdUsuario);

            
        }
    }
}

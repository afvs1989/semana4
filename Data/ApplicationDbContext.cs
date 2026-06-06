using Microsoft.EntityFrameworkCore;
using tarea2_clientes.Models;

namespace tarea2_clientes.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<HuespedModel> Huespedes { get; set; }
        public DbSet<HabitacionModel> Habitaciones { get; set; }
        public DbSet<ReservaModel> Reservas { get; set; }
        public DbSet<PagoModel> Pagos { get; set; }
        public DbSet<EmpleadoModel> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservaModel>()
                .HasOne(r => r.Huesped)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HuespedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReservaModel>()
                .HasOne(r => r.Habitacion)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HabitacionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReservaModel>()
                .HasOne(r => r.Empleado)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PagoModel>()
                .HasOne(p => p.Reserva)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ReservaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

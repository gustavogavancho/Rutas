using Microsoft.EntityFrameworkCore;

namespace Rutas.Models
{
    public class ProjectContext : DbContext
    {
        public DbSet<Tecnicos> Tecnicos { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<Proyectos> Proyectos { get; set; }
        public DbSet<PartNumbers> PartNumbers { get; set; }
        public DbSet<Almacenes> Almacenes { get; set; }
        public DbSet<Locaciones> Locaciones { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Rutas> Rutas { get; set; }
        public DbSet<SelectedLocalidades> SelectedLocalidades { get; set; }
        public DbSet<SelectedInventario> SelectedInventario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ProjectDB.db");
        }
    }
}

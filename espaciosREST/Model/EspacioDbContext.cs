using Microsoft.EntityFrameworkCore;

namespace EventosRest.Model
{
    public class EspacioDbContext : DbContext
    {
        public EspacioDbContext(DbContextOptions<EspacioDbContext> options) : base(options) { }
        public DbSet<Espacio> Espacios { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;

namespace EventosRest.Model
{
    public class HorarioEspacioDbContext : DbContext
    {
        public HorarioEspacioDbContext(DbContextOptions<HorarioEspacioDbContext> options) : base(options) { }
        public DbSet<HorarioEspacio> HorariosEspacios { get; set; }
    }
}
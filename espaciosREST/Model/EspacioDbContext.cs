    using Microsoft.EntityFrameworkCore;

    namespace EventosRest.Model
    {
        public class EspacioDbContext : DbContext
        {
            public EspacioDbContext(DbContextOptions<EspacioDbContext> options) : base(options) { }
            public DbSet<Espacio> Espacios { get; set; }
            public DbSet<HorarioEspacio> HorariosEspacios { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Espacio>()
                    .HasMany(e => e.Horarios)  
                    .WithOne()  
                    .HasForeignKey(h => h.espacio_id)  
                    .OnDelete(DeleteBehavior.Cascade);  

            base.OnModelCreating(modelBuilder);
            }
        }
    }
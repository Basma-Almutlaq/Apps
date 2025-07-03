using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
        public DbSet<Patients> Patient { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }

}
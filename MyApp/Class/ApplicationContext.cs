using Microsoft.EntityFrameworkCore;
using MyApp.Models;

    public class ApplicationContext : DbContext
    {
        public DbSet<HTMLPage> HtmlPages { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=praj");
        }
    }

using Microsoft.EntityFrameworkCore;

namespace PanamaPrintApp.Models
{
    public class CompanyContext : DbContext 
    {
        public CompanyContext(DbContextOptions<CompanyContext> options) :base(options) {  }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Model> Models { get; set; }
 
        public DbSet<ModelList> ModelList { get; set; }
    }
}

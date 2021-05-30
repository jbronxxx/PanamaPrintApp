using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PanamaPrintApp.Models
{
    public class UserContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public UserContext(DbContextOptions<UserContext> options) : base(options) { _options = options; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

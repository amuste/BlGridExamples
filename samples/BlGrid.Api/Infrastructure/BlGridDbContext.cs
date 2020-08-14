using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlGrid.Api.Infrastructure
{
    public class BlGridDbContext : DbContext, IBlGridDbContext
    {
        public BlGridDbContext(DbContextOptions<BlGridDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public DatabaseFacade GetDatabase()
        {
            return base.Database;
        }

        public DbContext GetDbContext()
        {
            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

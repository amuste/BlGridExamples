using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlGrid.Api.Infrastructure
{
    public interface IBlGridDbContext
    {
        DbSet<Person> Persons { get; set; }

        DbContext GetDbContext();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade GetDatabase();
    }
}

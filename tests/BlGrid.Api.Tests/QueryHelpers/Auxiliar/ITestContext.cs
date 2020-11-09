using System.Threading;
using System.Threading.Tasks;
using BlGrid.Api.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlGrid.Api.Infrastructure
{
    public interface ITestContext
    {
        DbSet<TestEntity> Entities { get; set; }

        DbContext GetDbContext();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade GetDatabase();
    }
}

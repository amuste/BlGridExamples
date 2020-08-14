using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlGrid.Api.Infrastructure
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {

            builder.HasKey(p => p.PersonId);

            builder.Property(p => p.PersonId).ValueGeneratedOnAdd();

            builder.Property(f => f.Registered).HasColumnType("datetime2");
        }
    }
}

using KayordKit.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KayordKit.Data.Configuration;
public class DataProviderConfiguration : IEntityTypeConfiguration<DataProvider>
{
    public void Configure(EntityTypeBuilder<DataProvider> builder)
    {
        builder.Property(t => t.ConnectionString).IsRequired();
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(250).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

    }
}

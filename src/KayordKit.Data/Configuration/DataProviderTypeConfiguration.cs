using KayordKit.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KayordKit.Data.Configuration;
public class DataProviderTypeConfiguration : IEntityTypeConfiguration<DataProviderType>
{
    public void Configure(EntityTypeBuilder<DataProviderType> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(250).IsRequired();
    }
}

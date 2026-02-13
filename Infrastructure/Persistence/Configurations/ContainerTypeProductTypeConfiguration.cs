using Domain.Entities;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ContainerTypeProductTypeConfiguration : IEntityTypeConfiguration<ContainerTypeProductType>
{
    public void Configure(EntityTypeBuilder<ContainerTypeProductType> builder)
    {
        builder.HasKey(x => new { x.ContainerTypeId, x.ProductTypeId });

        builder.Property(x => x.ContainerTypeId)
            .IsRequired();

        builder.Property(x => x.ProductTypeId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.HasOne(x => x.ContainerType)
           .WithMany(x => x.ProductTypes)
           .HasForeignKey(x => x.ContainerTypeId)
           .HasConstraintName("fk_container_type_product_types_container_type_id")
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ProductType)
            .WithMany(x => x.ContainerTypes)
            .HasForeignKey(x => x.ProductTypeId)
            .HasConstraintName("fk_container_type_product_types_product_type_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Domain.Entities;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class HistoriesConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ContainerId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnType("int")
            .IsRequired(false);

        builder.Property(x => x.UnitId)
            .IsRequired(false);

        builder.Property(x => x.ProductId)
            .IsRequired(false);

        builder.Property(x => x.ActionType)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.FromDate)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnType("varchar(500)")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.ChangedBy)
            .IsRequired(false);

        builder.HasOne(x => x.Container)
            .WithMany()
            .HasForeignKey(x => x.ContainerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
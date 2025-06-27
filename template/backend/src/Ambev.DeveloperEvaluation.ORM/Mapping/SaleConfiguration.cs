using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();         
        builder.Property(c => c.OrderId).IsRequired();
        builder.HasOne(c => c.Order)
               .WithMany()
               .HasForeignKey(c => c.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(20);
        builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(50);
        builder.Property(s => s.SaleDate).IsRequired();
        builder.Property(s => s.BranchName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.IsCancelled).IsRequired();
        builder.HasMany(c => c.Items)
               .WithOne(c => c.Sale)
               .HasForeignKey(i => i.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
               
    }
}

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
        builder.Property(i => i.ProductId).IsRequired();
        builder.HasOne(i => i.Product)
              .WithMany()
              .HasForeignKey(i => i.ProductId)
              .OnDelete(DeleteBehavior.Restrict);
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(i => i.Discount).HasColumnType("decimal(5,2)");
        builder.Property(i => i.Total).HasColumnType("decimal(18,2)");

    }
}

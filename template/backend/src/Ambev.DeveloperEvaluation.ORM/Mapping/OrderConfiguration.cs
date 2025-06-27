using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.CartId).IsRequired();
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(50);
        builder.Property(o => o.BranchName).IsRequired().HasMaxLength(200);
        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.UpdatedAt);
        builder.HasOne(o => o.Cart)
               .WithMany()
               .HasForeignKey(o => o.CartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.UserId).IsRequired();
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.HasMany(c => c.Items)
               .WithOne(i => i.Cart)
               .HasForeignKey(i => i.CartId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
        builder.Property(i => i.ProductId).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.UnitPrice);
        builder.Property(i => i.Discount);
        builder.Property(i => i.Total);
        builder.HasOne(i => i.Product)
               .WithMany()
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

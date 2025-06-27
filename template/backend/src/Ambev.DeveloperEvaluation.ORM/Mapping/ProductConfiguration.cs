using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    private static readonly DateTime _baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.Category).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Image).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);

        // Seed com dados iniciais        
        builder.HasData(BuildInitialLoadData());
    }

    private static List<Product> BuildInitialLoadData()
    {
        return [

            new Product
            {
                Id = 1,
                Title = "Brahma Duplo Malte 350ml",
                Description = "Cerveja Brahma Duplo Malte Lata 350ml - Pack com 12 unidades",
                Price = 35.88M,
                Category = "Cervejas",
                Image = "https://www.ambev.com.br/images/products/brahma-duplo-malte-350ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 2,
                Title = "Stella Artois 275ml",
                Description = "Cerveja Stella Artois Long Neck 275ml - Pack com 6 unidades",
                Price = 31.90M,
                Category = "Cervejas Premium",
                Image = "https://www.ambev.com.br/images/products/stella-artois-275ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 3,
                Title = "Corona Extra 330ml",
                Description = "Cerveja Corona Extra Long Neck 330ml - Pack com 6 unidades",
                Price = 39.90M,
                Category = "Cervejas Premium",
                Image = "https://www.ambev.com.br/images/products/corona-extra-330ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 4,
                Title = "Guaraná Antarctica 2L",
                Description = "Refrigerante Guaraná Antarctica 2L - Pack com 6 unidades",
                Price = 42.00M,
                Category = "Refrigerantes",
                Image = "https://www.ambev.com.br/images/products/guarana-antarctica-2l.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 5,
                Title = "Pepsi 350ml",
                Description = "Refrigerante Pepsi Lata 350ml - Pack com 12 unidades",
                Price = 28.80M,
                Category = "Refrigerantes",
                Image = "https://www.ambev.com.br/images/products/pepsi-350ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 6,
                Title = "H2OH! Limão 500ml",
                Description = "Bebida H2OH! Limão 500ml - Pack com 6 unidades",
                Price = 24.00M,
                Category = "Águas Saborizadas",
                Image = "https://www.ambev.com.br/images/products/h2oh-limao-500ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 7,
                Title = "Gatorade Laranja 500ml",
                Description = "Isotônico Gatorade Laranja 500ml - Pack com 6 unidades",
                Price = 32.90M,
                Category = "Isotônicos",
                Image = "https://www.ambev.com.br/images/products/gatorade-laranja-500ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 8,
                Title = "Budweiser 350ml",
                Description = "Cerveja Budweiser Lata 350ml - Pack com 12 unidades",
                Price = 47.88M,
                Category = "Cervejas Premium",
                Image = "https://www.ambev.com.br/images/products/budweiser-350ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 9,
                Title = "Original 600ml",
                Description = "Cerveja Original Garrafa 600ml - Pack com 12 unidades",
                Price = 89.88M,
                Category = "Cervejas",
                Image = "https://www.ambev.com.br/images/products/original-600ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            },
            new Product
            {
                Id = 10,
                Title = "Antarctica Sub Zero 473ml",
                Description = "Cerveja Antarctica Sub Zero Lata 473ml - Pack com 12 unidades",
                Price = 35.88M,
                Category = "Cervejas",
                Image = "https://www.ambev.com.br/images/products/antarctica-subzero-473ml.jpg",
                CreatedAt = _baseDate,
                UpdatedAt = null
            }
        ];
    }
}
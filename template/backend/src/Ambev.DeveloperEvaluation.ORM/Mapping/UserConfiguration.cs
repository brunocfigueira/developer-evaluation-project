using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Seed com dados iniciais        
        builder.HasData(BuildInitialLoadData());
    }

    private List<User> BuildInitialLoadData()
    {
        return [
                new User
                {
                    Id = 1,
                    Username = "User.Customer",
                    Email = "user.customer@ambev.com.br",
                    Password = _passwordHasher.HashPassword(null, "Customer@123456"),
                    Phone = "99999999999",
                    Role = UserRole.Customer,
                    Status = UserStatus.Active
                },
                new User
                {
                    Id = 2,
                    Username = "User.Manager",
                    Email = "user.manager@ambev.com.br",
                    Password = _passwordHasher.HashPassword(null, "Manager@123456"),
                    Phone = "99999999999",
                    Role = UserRole.Manager,
                    Status = UserStatus.Active
                },
                new User
                {
                    Id = 3,
                    Username = "User.Admin",
                    Email = "user.admin@ambev.com.br",
                    Password = _passwordHasher.HashPassword(null, "Admin@123456"),
                    Phone = "99999999999",
                    Role = UserRole.Admin,
                    Status = UserStatus.Active
                },
        ];
    }
}

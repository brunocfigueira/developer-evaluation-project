using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class CartTests
    {
        [Fact]
        public void CalculateTotalAmount_ShouldReturnZero_WhenNoItems()
        {
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Status = CartStatus.Open,
                User = new User { Id = 1, Username = "Test User", Email = "test@email.com", Phone = "(11) 99999-9999", Password = "Password1!", Role = UserRole.Customer, Status = UserStatus.Active, CreatedAt = DateTime.UtcNow },
                Items = new List<CartItem>()
            };
            cart.CalculateTotalAmount().Should().Be(0);
        }

        [Fact]
        public void CalculateTotalAmount_ShouldReturnSumOfItemTotals()
        {
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Status = CartStatus.Open,
                User = new User { Id = 1, Username = "Test User", Email = "test@email.com", Phone = "(11) 99999-9999", Password = "Password1!", Role = UserRole.Customer, Status = UserStatus.Active, CreatedAt = DateTime.UtcNow },
                Items = new List<CartItem>
                {
                    new CartItem { Id = 1, Quantity = 2, Total = 20 },
                    new CartItem { Id = 2, Quantity = 1, Total = 10 },
                    new CartItem { Id = 3, Quantity = 3, Total = 30 }
                }
            };
            cart.CalculateTotalAmount().Should().Be(60);
        }

        [Fact]
        public void Cart_ShouldSetCreatedAtToUtcNowByDefault()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Status = CartStatus.Open,
                User = new User { Id = 1, Username = "Test User", Email = "test@email.com", Phone = "(11) 99999-9999", Password = "Password1!", Role = UserRole.Customer, Status = UserStatus.Active, CreatedAt = DateTime.UtcNow }
            };
            var after = DateTime.UtcNow.AddSeconds(1);
            cart.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        }

        [Fact]
        public void Cart_ShouldAllowSettingUpdatedAt()
        {
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Status = CartStatus.Open,
                User = new User { Id = 1, Username = "Test User", Email = "test@email.com", Phone = "(11) 99999-9999", Password = "Password1!", Role = UserRole.Customer, Status = UserStatus.Active, CreatedAt = DateTime.UtcNow },
                UpdatedAt = DateTime.UtcNow
            };
            cart.UpdatedAt.Should().NotBeNull();
        }
    }
}

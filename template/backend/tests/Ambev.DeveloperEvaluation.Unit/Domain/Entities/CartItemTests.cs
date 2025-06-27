using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class CartItemTests
    {
        [Fact]
        public void ApplyDiscountRules_ShouldApplyNoDiscount_WhenQuantityLessThan4()
        {
            var product = new Product { Id = 1, Price = 10m, Title = "Test", Description = "desc", Category = "cat", Image = "img", CreatedAt = DateTime.UtcNow };
            var item = new CartItem { Product = product, Quantity = 3 };
            item.ApplyDiscountRules();
            item.Discount.Should().Be(0);
            item.UnitPrice.Should().Be(10m);
            item.Total.Should().Be(30m);
        }

        [Fact]
        public void ApplyDiscountRules_ShouldApply10PercentDiscount_WhenQuantityBetween4And9()
        {
            var product = new Product { Id = 1, Price = 10m, Title = "Test", Description = "desc", Category = "cat", Image = "img", CreatedAt = DateTime.UtcNow };
            var item = new CartItem { Product = product, Quantity = 5 };
            item.ApplyDiscountRules();
            item.Discount.Should().Be(0.10m);
            item.UnitPrice.Should().Be(10m);
            item.Total.Should().Be(45m);
        }

        [Fact]
        public void ApplyDiscountRules_ShouldApply20PercentDiscount_WhenQuantityBetween10And20()
        {
            var product = new Product { Id = 1, Price = 10m, Title = "Test", Description = "desc", Category = "cat", Image = "img", CreatedAt = DateTime.UtcNow };
            var item = new CartItem { Product = product, Quantity = 15 };
            item.ApplyDiscountRules();
            item.Discount.Should().Be(0.20m);
            item.UnitPrice.Should().Be(10m);
            item.Total.Should().Be(120m);
        }

        [Fact]
        public void ApplyDiscountRules_ShouldThrow_WhenQuantityGreaterThan20()
        {
            var product = new Product { Id = 1, Price = 10m, Title = "Test", Description = "desc", Category = "cat", Image = "img", CreatedAt = DateTime.UtcNow };
            var item = new CartItem { Product = product, Quantity = 21 };
            Action act = () => item.ApplyDiscountRules();
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("It is not possible to sell more than 20 identical products.");
        }

        [Fact]
        public void ApplyDiscountRules_ShouldThrow_WhenProductIsNull()
        {
            var item = new CartItem { Product = null, Quantity = 2 };
            Action act = () => item.ApplyDiscountRules();
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("It was not possible to recover the price of the product.");
        }
    }
}

using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public CartStatus Status { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
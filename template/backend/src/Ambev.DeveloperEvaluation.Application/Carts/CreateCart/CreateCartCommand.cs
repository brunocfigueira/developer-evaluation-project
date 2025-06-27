using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartCommand : IRequest<CreateCartResult>
{
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public CartStatus Status { get; set; } = CartStatus.Open;
    public IEnumerable<CreateCartItemsCommand> Items { get; set; } = new List<CreateCartItemsCommand>();
}

public class CreateCartItemsCommand
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }  
}

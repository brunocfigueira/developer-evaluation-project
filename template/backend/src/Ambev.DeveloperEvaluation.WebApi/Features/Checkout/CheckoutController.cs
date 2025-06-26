using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Responses;
using Ambev.DeveloperEvaluation.WebApi.Features.Checkout.Requests;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Checkout;
[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IBus _bus;

    public CheckoutController(IMediator mediator, IMapper mapper, IBus bus)
    {
        _mediator = mediator;
        _mapper = mapper;
        _bus = bus;
    }
    /**
     * TO DO: Adicionar outros metdos inerente ao checkout, como por exemplo:
     * 
     * StartCheckout	POST	/checkout/start	- Iniciar o processo de checkout com base no carrinho atual do usuário.
     * ValidateAddress	POST	/checkout/address - Validar e registrar endereço de entrega e cobrança.
     * ApplyCoupon	POST	/checkout/coupon - Aplicar e validar um código promocional.
     * SelectPaymentMethod	POST	/checkout/payment-method - Registrar o meio de pagamento escolhido (PIX, cartão, boleto, etc.).
     * ReviewOrder	GET	/checkout/review - Retornar um resumo completo do pedido antes de finalizar.
     * GetCheckoutStatus	GET	/checkout/status - Consultar o status atual do checkout (útil para retomada em apps ou web).
     * CancelCheckout	POST - Permitir o cancelamento do processo antes da finalização.
     * SaveProgress	POST - Salvar o progresso do checkout (útil para apps/mobile).
     * EstimateShipping	GET ou POST - Calcular o valor do frete com base no endereço e produtos.
     * UploadPaymentProof	POST - Upload de comprovante (ex: transferência bancária manual).
     * RetryPayment	POST - Reprocessar o pagamento após falha (banco fora do ar, etc).
     * 
     */
    [HttpPost("ConfirmOrder")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        // criar um pedido a partir do carrinho
        var command = _mapper.Map<CreateOrderCommand>(request);
        var order = await _mediator.Send(command, cancellationToken);

        var message = _mapper.Map<OrderConfirmedMessage>(request);
        message.OrderId = order.Id;
        message.CreatedAt = order.CreatedAt;

        // processar o pedido assincrono
        await _bus.Send(message);

        var response = new ApiResponseWithData<CreateOrderResult>
        {
            Success = true,
            Message = "Order created successfully",
            Data = order

        };

        return Ok(response);
    }
}


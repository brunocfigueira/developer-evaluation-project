namespace Ambev.DeveloperEvaluation.Domain.Enums;

public enum CartStatus
{
    Unknown = 0,
    Open,              // Cliente está adicionando/removendo produtos
    PendingCheckout,   // Checkout iniciado
    Completed,         // Compra finalizada
    Abandoned,         // Cliente sumiu
    Cancelled,         // Cancelado manualmente
    Expired,           // Tempo expirado (inatividade)
    Error              // Alguma falha (estoque, pagamento, etc)
}


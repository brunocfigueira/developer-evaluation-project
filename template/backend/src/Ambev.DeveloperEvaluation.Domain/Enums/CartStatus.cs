namespace Ambev.DeveloperEvaluation.Domain.Enums;

public enum CartStatus
{
    Unknown = 0,
    Open,              
    PendingCheckout,
    AwaitingPayment,
    Completed,         
    Abandoned,         
    Cancelled,         
    Expired,           
    Error,             
}


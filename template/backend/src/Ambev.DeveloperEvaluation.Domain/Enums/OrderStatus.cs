namespace Ambev.DeveloperEvaluation.Domain.Enums;

public enum OrderStatus
{
    Unknown = 0,
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned,
    Completed,
    Refunded,
    OnHold,
    AwaitingPayment,
    AwaitingShipment,
    AwaitingPickup,
    AwaitingConfirmation,
    PartiallyShipped,
    PartiallyDelivered,
    PartiallyRefunded,
    PartiallyCancelled,
    AwaitingReview,
    AwaitingApproval,
    InTransit,
    Delayed,
    Rescheduled,
    Expired
}


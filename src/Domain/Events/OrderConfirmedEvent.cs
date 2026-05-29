namespace SARR.Domain.Events;

using SARR.Domain.ValueObjects;

public sealed class OrderConfirmedEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public Guid CustomerId { get; }
    public Money TotalAmount { get; }
    public int DiscountedItemsCount { get; }
    public DateTime OccurredAt { get; }

    public OrderConfirmedEvent(
        Guid orderId,
        Guid customerId,
        Money totalAmount,
        int discountedItemsCount,
        DateTime occurredAt)
    {
        AggregateId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        DiscountedItemsCount = discountedItemsCount;
        OccurredAt = occurredAt;
    }
}

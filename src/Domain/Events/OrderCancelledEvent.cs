namespace SARR.Domain.Events;

public sealed class OrderCancelledEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public Guid CustomerId { get; }
    public DateTime OccurredAt { get; }

    public OrderCancelledEvent(Guid orderId, Guid customerId, DateTime occurredAt)
    {
        AggregateId = orderId;
        CustomerId = customerId;
        OccurredAt = occurredAt;
    }
}

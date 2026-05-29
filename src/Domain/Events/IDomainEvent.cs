namespace SARR.Domain.Events;

public interface IDomainEvent
{
    Guid AggregateId { get; }
    DateTime OccurredAt { get; }
}

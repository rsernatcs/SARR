namespace SARR.Domain.Exceptions;

public sealed class DiscountLimitExceededException : DomainException
{
    public Guid CustomerId { get; }
    public int CurrentCount { get; }
    public int MaxAllowed { get; }

    public DiscountLimitExceededException(
        Guid customerId,
        int currentCount,
        int maxAllowed)
        : base($"El cliente {customerId} ha alcanzado su límite de {maxAllowed} artículos con descuento en los últimos 30 días. Actualmente tiene {currentCount}.")
    {
        CustomerId = customerId;
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }
}

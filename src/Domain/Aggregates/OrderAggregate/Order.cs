namespace SARR.Domain.Aggregates.OrderAggregate;

using SARR.Domain.ValueObjects;
using SARR.Domain.Events;

public sealed class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money SubTotal { get; private set; }
    public Money Total { get; private set; }
    public int DiscountedItemsCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Order() { }

    public static Order Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("El ID del cliente no puede estar vacío.", nameof(customerId));

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            SubTotal = Money.Create(0, "USD"),
            Total = Money.Create(0, "USD"),
            DiscountedItemsCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        return order;
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("No se pueden agregar items a una orden que no está pendiente.");

        _items.Add(item);
        RecalculateTotals();
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("No se pueden eliminar items de una orden que no está pendiente.");

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
            throw new InvalidOperationException("El item no existe en la orden.");

        _items.Remove(item);
        RecalculateTotals();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Solo se pueden confirmar órdenes pendientes.");

        if (!_items.Any())
            throw new InvalidOperationException("La orden debe tener al menos un item.");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;

        // Emitir evento de dominio
        _domainEvents.Add(new OrderConfirmedEvent(
            Id,
            CustomerId,
            Total,
            DiscountedItemsCount,
            DateTime.UtcNow
        ));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("La orden ya está cancelada.");

        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("No se puede cancelar una orden completada.");

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        _domainEvents.Add(new OrderCancelledEvent(Id, CustomerId, DateTime.UtcNow));
    }

    private void RecalculateTotals()
    {
        SubTotal = _items.Aggregate(
            Money.Create(0, "USD"),
            (acc, item) => acc.Add(item.SubTotal)
        );

        Total = _items.Aggregate(
            Money.Create(0, "USD"),
            (acc, item) => acc.Add(item.Total)
        );

        DiscountedItemsCount = _items.Count(i => i.HasDiscount);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

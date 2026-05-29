namespace SARR.Domain.Aggregates.OrderAggregate;

using SARR.Domain.ValueObjects;

public sealed class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public ProductQuantity Quantity { get; private set; }
    public Discount AppliedDiscount { get; private set; }
    public Money SubTotal { get; private set; }
    public Money Total { get; private set; }

    private OrderItem() { }

    public static OrderItem Create(
        Guid productId,
        string productName,
        Money unitPrice,
        ProductQuantity quantity,
        Discount discount)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("El ID del producto no puede estar vacío.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(productName));

        if (unitPrice is null)
            throw new ArgumentNullException(nameof(unitPrice));

        if (quantity is null)
            throw new ArgumentNullException(nameof(quantity));

        if (discount is null)
            throw new ArgumentNullException(nameof(discount));

        var subTotal = Money.Create(unitPrice.Amount * quantity.Value, unitPrice.Currency);
        var total = discount.HasDiscount 
            ? subTotal.ApplyDiscount(discount.Percentage) 
            : subTotal;

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            AppliedDiscount = discount,
            SubTotal = subTotal,
            Total = total
        };
    }

    public bool HasDiscount => AppliedDiscount.HasDiscount;
}

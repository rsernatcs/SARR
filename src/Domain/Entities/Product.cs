namespace SARR.Domain.Entities;

using SARR.Domain.ValueObjects;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public static Product Create(
        string name,
        string description,
        Money price,
        int stockQuantity,
        Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía.", nameof(description));

        if (price is null)
            throw new ArgumentNullException(nameof(price));

        if (stockQuantity < 0)
            throw new ArgumentException("El stock no puede ser negativo.", nameof(stockQuantity));

        if (categoryId == Guid.Empty)
            throw new ArgumentException("La categoría es requerida.", nameof(categoryId));

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Stock insuficiente.");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReplenishStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice is null)
            throw new ArgumentNullException(nameof(newPrice));

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

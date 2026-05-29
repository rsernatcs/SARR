namespace SARR.Domain.ValueObjects;

public sealed class ProductQuantity : IEquatable<ProductQuantity>
{
    public int Value { get; }

    private ProductQuantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(value));

        Value = value;
    }

    public static ProductQuantity Create(int quantity)
    {
        return new ProductQuantity(quantity);
    }

    public bool Equals(ProductQuantity? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProductQuantity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(ProductQuantity? left, ProductQuantity? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(ProductQuantity? left, ProductQuantity? right)
    {
        return !(left == right);
    }

    public override string ToString() => Value.ToString();
}

namespace SARR.Domain.ValueObjects;

public sealed class Discount : IEquatable<Discount>
{
    public decimal Percentage { get; }
    public string Reason { get; }

    private Discount(decimal percentage, string reason)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("El porcentaje debe estar entre 0 y 100.", nameof(percentage));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("La razón del descuento no puede estar vacía.", nameof(reason));

        Percentage = percentage;
        Reason = reason;
    }

    public static Discount Create(decimal percentage, string reason)
    {
        return new Discount(percentage, reason);
    }

    public static Discount NoDiscount()
    {
        return new Discount(0, "Sin descuento");
    }

    public bool HasDiscount => Percentage > 0;

    public bool Equals(Discount? other)
    {
        return other is not null && Percentage == other.Percentage && Reason == other.Reason;
    }

    public override bool Equals(object? obj)
    {
        return obj is Discount other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Percentage, Reason);
    }

    public override string ToString() => $"{Percentage}% - {Reason}";
}

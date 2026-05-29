namespace SARR.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("El monto no puede ser negativo.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new ArgumentException("La moneda debe ser un código ISO válido.", nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "USD")
    {
        return new Money(amount, currency);
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("No se pueden sumar dineros con diferentes monedas.");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("No se pueden restar dineros con diferentes monedas.");

        var result = Amount - other.Amount;
        if (result < 0)
            throw new InvalidOperationException("El resultado no puede ser negativo.");

        return new Money(result, Currency);
    }

    public Money ApplyDiscount(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.", nameof(discountPercentage));

        var discountedAmount = Amount * (1 - discountPercentage / 100);
        return new Money(discountedAmount, Currency);
    }

    public bool Equals(Money? other)
    {
        return other is not null && Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public static bool operator ==(Money? left, Money? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}

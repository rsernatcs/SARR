namespace SARR.Domain.ValueObjects;

using System.Text.RegularExpressions;

public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    private Email(string value)
    {
        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("El formato del email no es válido.", nameof(value));

        Value = value.ToLowerInvariant();
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío.", nameof(value));

        return new Email(value);
    }

    public bool Equals(Email? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Email? left, Email? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Email? left, Email? right)
    {
        return !(left == right);
    }

    public override string ToString() => Value;
}

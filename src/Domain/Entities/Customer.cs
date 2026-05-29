namespace SARR.Domain.Entities;

using SARR.Domain.ValueObjects;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public string FullName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Customer() { }

    public static Customer Create(string email, string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo no puede estar vacío.", nameof(fullName));

        return new Customer
        {
            Id = Guid.NewGuid(),
            Email = Email.Create(email),
            FullName = fullName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void UpdateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo no puede estar vacío.", nameof(fullName));

        FullName = fullName;
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

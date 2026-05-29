namespace SARR.Domain.Exceptions;

public sealed class InsufficientStockException : DomainException
{
    public Guid ProductId { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }

    public InsufficientStockException(
        Guid productId,
        int requestedQuantity,
        int availableQuantity)
        : base($"Stock insuficiente para el producto {productId}. Solicitado: {requestedQuantity}, disponible: {availableQuantity}.")
    {
        ProductId = productId;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = availableQuantity;
    }
}

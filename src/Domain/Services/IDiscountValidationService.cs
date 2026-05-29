namespace SARR.Domain.Services;

public interface IDiscountValidationService
{
    /// <summary>
    /// Valida si el cliente puede aplicar un descuento en un nuevo artículo.
    /// Si ha alcanzado el límite (5), devuelve false.
    /// </summary>
    Task<bool> CanApplyDiscountAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la cantidad de artículos con descuento comprados en los últimos 30 días.
    /// </summary>
    Task<int> GetDiscountedItemsCountInLast30DaysAsync(Guid customerId, CancellationToken cancellationToken = default);
}

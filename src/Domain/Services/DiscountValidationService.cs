namespace SARR.Domain.Services;

using SARR.Domain.Repositories;

public sealed class DiscountValidationService : IDiscountValidationService
{
    private const int MaxDiscountedItemsPerMonth = 5;
    private readonly IOrderRepository _orderRepository;

    public DiscountValidationService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> CanApplyDiscountAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var count = await GetDiscountedItemsCountInLast30DaysAsync(customerId, cancellationToken);
        return count < MaxDiscountedItemsPerMonth;
    }

    public async Task<int> GetDiscountedItemsCountInLast30DaysAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetConfirmedOrdersInLast30DaysAsync(customerId, cancellationToken);
        return orders.SelectMany(o => o.Items).Count(i => i.HasDiscount);
    }
}

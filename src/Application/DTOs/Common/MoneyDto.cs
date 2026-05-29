namespace SARR.Application.DTOs.Common;

public sealed record MoneyDto(
    decimal Amount,
    string Currency = "USD"
);

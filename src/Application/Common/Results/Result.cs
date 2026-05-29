namespace SARR.Application.Common.Results;

public sealed class Result
{
    private Result(bool isSuccess, string error, string? successMessage = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        SuccessMessage = successMessage;
    }

    public bool IsSuccess { get; }
    public string Error { get; }
    public string? SuccessMessage { get; }

    public static Result Success(string? message = null)
    {
        return new Result(true, string.Empty, message);
    }

    public static Result Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("El error no puede estar vacío.", nameof(error));

        return new Result(false, error);
    }

    public TResult Match<TResult>(
        Func<string?, TResult> onSuccess,
        Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(SuccessMessage) : onFailure(Error);
    }

    public async Task<TResult> MatchAsync<TResult>(
        Func<string?, Task<TResult>> onSuccess,
        Func<string, Task<TResult>> onFailure)
    {
        return IsSuccess ? await onSuccess(SuccessMessage) : await onFailure(Error);
    }

    public void Match(
        Action<string?> onSuccess,
        Action<string> onFailure)
    {
        if (IsSuccess)
            onSuccess(SuccessMessage);
        else
            onFailure(Error);
    }

    public async Task MatchAsync(
        Func<string?, Task> onSuccess,
        Func<string, Task> onFailure)
    {
        if (IsSuccess)
            await onSuccess(SuccessMessage);
        else
            await onFailure(Error);
    }
}

public sealed class Result<T>
{
    private Result(bool isSuccess, T? data, string error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T? Data { get; }
    public string Error { get; }

    public static Result<T> Success(T data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data), "Los datos no pueden ser nulos.");

        return new Result<T>(true, data, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("El error no puede estar vacío.", nameof(error));

        return new Result<T>(false, default, error);
    }

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Data!) : onFailure(Error);
    }

    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<string, Task<TResult>> onFailure)
    {
        return IsSuccess ? await onSuccess(Data!) : await onFailure(Error);
    }

    public void Match(
        Action<T> onSuccess,
        Action<string> onFailure)
    {
        if (IsSuccess)
            onSuccess(Data!);
        else
            onFailure(Error);
    }

    public async Task MatchAsync(
        Func<T, Task> onSuccess,
        Func<string, Task> onFailure)
    {
        if (IsSuccess)
            await onSuccess(Data!);
        else
            await onFailure(Error);
    }
}

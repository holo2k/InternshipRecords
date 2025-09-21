using FluentValidation;
using MediatR;
using Shared.Models;

namespace InternshipRecords.Web.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count <= 0)
            return await next(cancellationToken);

        var validationErrors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray()
            );

        var mbError = new MbError(
            "ValidationFailure",
            "Ошибка во время валидации",
            validationErrors
        );

        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(MbResult<>))
            throw new InvalidOperationException("ValidationBehavior expects TResponse to be MbResult<T>.");

        var failMethod = responseType.GetMethod("Fail", new[] { typeof(MbError) })!;
        var result = failMethod.Invoke(null, new object[] { mbError })!;

        _logger.LogError("Validation failed: {ErrorCode}, Message: {ErrorMessage}, Request: {@Request}",
            mbError.Code,
            mbError.Message,
            request);

        return (TResponse)result;
    }
}
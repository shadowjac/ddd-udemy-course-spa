using Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<TRequest> _logger;


    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        
        _logger.LogInformation($"***** ValidationBehavior: {typeof(TRequest).Name}");
        if (!_validators.Any())
        {
            _logger.LogInformation($"***** ValidationBehavior: {typeof(TRequest).Name} - No validators found");
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var errors = _validators.Select(v => v.Validate(context))
            .Where(v => v.Errors.Any())
            .SelectMany(v => v.Errors)
            .Select(v => new ValidationError(
                v.PropertyName,
                v.ErrorMessage))
            .ToList();

        if (errors.Any())
        {
            _logger.LogError("***** Error {errors}", errors);
            throw new Exceptions.ValidationException(errors);
        }

        _logger.LogInformation($"***** ValidationBehavior: {typeof(TRequest).Name} - No errors (2)");
        return await next();
    }
}
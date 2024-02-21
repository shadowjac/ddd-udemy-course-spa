using Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;
        try
        {
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", name, request);
            var result = await next();
            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", name, result);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Command {CommandName} failed", name);
            throw;
        }
    }
}
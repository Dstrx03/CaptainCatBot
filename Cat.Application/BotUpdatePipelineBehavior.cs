using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cat.Application
{
    public class BotUpdatePipelineBehavior<TRequest, TUpdate> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IBotUpdateCommand<TUpdate>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BotUpdatePipelineBehavior<TRequest, TUpdate>> _logger;

        public BotUpdatePipelineBehavior(IServiceProvider serviceProvider, ILogger<BotUpdatePipelineBehavior<TRequest, TUpdate>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            _logger.LogTrace($"*** PipelineBehavior - TRequest:{typeof(TRequest).FullName} TUpdate:{typeof(TUpdate).FullName}");
            return next();
        }
    }
}

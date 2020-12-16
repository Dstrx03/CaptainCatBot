using Cat.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Application
{
    public class BotUpdatePipelineBehavior<TRequest, TUpdate> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IBotUpdateCommand<TUpdate>
    {
        private readonly IServiceProvider _serviceProvider;

        public BotUpdatePipelineBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            // todo: validation?

            var updatePreProcessor = (IBotUpdatePreProcessor<TUpdate>)_serviceProvider.GetService(typeof(IBotUpdatePreProcessor<TUpdate>));
            if (updatePreProcessor != null && updatePreProcessor.PreProcessingIsRequired(request.Update))
            {
                var shortCircuit = await updatePreProcessor.PreProcessUpdateAsync(request.Update);
                if (shortCircuit) return Unit.Value;
            }

            return await next();
        }
    }
}

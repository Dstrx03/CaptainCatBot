﻿using System.Threading;
using System.Threading.Tasks;
using Cat.Domain;
using MediatR;

namespace Cat.Application
{
    public abstract class BotUpdateCommandHandlerBase<TRequest, TUpdate> : IRequestHandler<TRequest>
        where TRequest : IBotUpdateCommand<TUpdate>
    {
        private readonly BotUpdateProcessor _updateProcessor;
        private readonly IBotUpdateContextFactory<TUpdate> _updateContextFactory;

        protected BotUpdateCommandHandlerBase(BotUpdateProcessor updateProcessor, IBotUpdateContextFactory<TUpdate> updateContextFactory)
        {
            _updateProcessor = updateProcessor;
            _updateContextFactory = updateContextFactory;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var updateContext = _updateContextFactory.CreateContext(request.Update);
            await _updateProcessor.ProcessUpdateAsync(updateContext);
            return Unit.Value;
        }
    }
}

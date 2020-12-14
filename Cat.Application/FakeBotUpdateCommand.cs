using System.Threading;
using System.Threading.Tasks;
using Cat.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cat.Application
{
    // todo: remove *************************************************************
    public interface ISomeInterface : IBotUpdateCommand<FakeBotUpdate> { }

    public class SomeUpdate
    {
        public string Msg { get; set; }
    }

    public class SomeCommand : IRequest /*IBotUpdateCommand<SomeUpdate>*/
    {
        public SomeCommand(string msg)
        {
            Update = new SomeUpdate { Msg = msg };
        }

        public SomeUpdate Update { get; set; }
    }

    public class SomeCommandHandler : IRequestHandler<SomeCommand>
    {
        private ILogger<SomeCommandHandler> _logger;

        public SomeCommandHandler(ILogger<SomeCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(SomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"*** SomeCommandHandler - Msg:{request?.Update.Msg}");
            return Task.FromResult(Unit.Value);
        }
    }
    // todo: remove *************************************************************

    public class FakeBotUpdateCommand : ISomeInterface /*IBotUpdateCommand<FakeBotUpdate>*/
    {
        public FakeBotUpdateCommand(FakeBotUpdate update)
        {
            Update = update;
        }

        public FakeBotUpdate Update { get; set; }
    }

    public class FakeBotUpdateCommandHandler : BotUpdateCommandHandlerBase<FakeBotUpdateCommand, FakeBotUpdate>
    {
        public FakeBotUpdateCommandHandler(BotUpdateProcessor updateProcessor, IBotUpdateContextFactory<FakeBotUpdate> updateContextFactory) 
            : base(updateProcessor, updateContextFactory)
        {
        }
    }
}

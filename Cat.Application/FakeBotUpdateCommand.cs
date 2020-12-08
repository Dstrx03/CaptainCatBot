using System.Threading;
using System.Threading.Tasks;
using Cat.Domain;
using MediatR;

namespace Cat.Application
{
    public class FakeBotUpdateCommand : IRequest
    {
        public FakeBotUpdateCommand(FakeBotUpdate update)
        {
            Update = update;
        }

        public FakeBotUpdate Update { get; set; }
    }

    public class FakeBotUpdateCommandHandler : IRequestHandler<FakeBotUpdateCommand>
    {
        private readonly BotUpdateProcessor _updateProcessor;
        private readonly IBotUpdateContextFactory<FakeBotUpdate> _updateContextFactory;
        private readonly IBotUpdateValidator<FakeBotUpdate> _updateValidator;

        public FakeBotUpdateCommandHandler(BotUpdateProcessor updateProcessor, IBotUpdateContextFactory<FakeBotUpdate> updateContextFactory, IBotUpdateValidator<FakeBotUpdate> updateValidator)
        {
            _updateProcessor = updateProcessor;
            _updateContextFactory = updateContextFactory;
            _updateValidator = updateValidator;
        }

        public async Task<Unit> Handle(FakeBotUpdateCommand request, CancellationToken cancellationToken)
        {
            //????????????????????????????????????????????????????????????????
            if (!await _updateValidator.ValidateUpdateAsync(request.Update))
            {
                return Unit.Value;
            }
            //????????????????????????????????????????????????????????????????

            // todo: this code is probably will be the same for each implementation of bot update command, move it to abstract class?
            var updateContext = _updateContextFactory.CreateContext(request.Update);
            await _updateProcessor.ProcessUpdateAsync(updateContext);
            return Unit.Value;
        }
    }
}

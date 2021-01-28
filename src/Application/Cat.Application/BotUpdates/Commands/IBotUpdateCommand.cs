using MediatR;

namespace Cat.Application.BotUpdates.Commands
{
    public interface IBotUpdateCommand<TUpdate> : IRequest
    {
        TUpdate Update { get; set; }
    }
}

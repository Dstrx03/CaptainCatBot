using MediatR;

namespace Cat.Application
{
    public interface IBotUpdateCommand<TUpdate> : IRequest
    {
        TUpdate Update { get; set; }
    }
}

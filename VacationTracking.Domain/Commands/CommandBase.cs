using MediatR;

namespace VacationTracking.Domain.Commands
{
    public class CommandBase<T> : IRequest<T> where T : class
    {
    }
}

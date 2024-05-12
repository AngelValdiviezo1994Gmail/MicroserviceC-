using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;

namespace Module.Cliente.Domain.CQRS.Command
{
    public class InactivarGeneroCommand : IRequest<Response<bool>>, ITracking
    {
        public Guid TrackingId { get; set; }

        public int GeneroId { get; set; }
    }
}

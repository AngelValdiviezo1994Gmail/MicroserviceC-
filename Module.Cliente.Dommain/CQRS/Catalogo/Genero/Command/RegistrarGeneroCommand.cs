using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Module.Cliente.Domain.Entities;

namespace Module.Cliente.Domain.CQRS.Command
{
    public class RegistrarGeneroCommand : Genero, IRequest<Response<Genero>>, ITracking
    {
        public Guid TrackingId { get; set; }
    }
}

using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Module.Cliente.Domain.Entities;

namespace Module.Cliente.Domain.CQRS.Query
{
    public class GetGeneros : IRequest<ResponsePaged<List<Genero>>>, ITracking
    {
        public Guid TrackingId { get; set; }
        public int Pagina { get; set; } = 1;
        public int Cantidad { get; set; } = 10;
        public List<Filtro>? Filtros { get; set; }
        public OrdenFiltro? Orden { get; set; }
        public int GeneroId { get; set; }
    }
}

using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Module.Security.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Security.Domain.CQRS.Usuarios.Query
{
    public class GetUsersQuery : IRequest<Response<List<User>>>, ITracking
    {
        public Guid TrackingId { get; set; }
        public List<Filtro>? Filtros { get; set; }
        public OrdenFiltro? Orden { get; set; }
        public string CriterioBusqueda { get; set; }
        public int Pagina { get; set; } = 1;
        public int Cantidad { get; set; } = 10;
    }
}

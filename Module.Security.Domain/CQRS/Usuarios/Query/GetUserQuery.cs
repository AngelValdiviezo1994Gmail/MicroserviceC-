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
    public class GetUserQuery : IRequest<Response<User>>, ITracking
    {
        public Guid TrackingId
        { get; set; }
        public int UsuarioId
        { get; set; }

    }
}

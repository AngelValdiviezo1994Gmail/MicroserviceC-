using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Security.Domain.CQRS.Usuarios.Command
{
    public class InactivarUsuarioCommand : IRequest<Response<bool>>, ITracking
    {
        public Guid TrackingId { get; set; }

        public int Id { get; set; }

    }
}

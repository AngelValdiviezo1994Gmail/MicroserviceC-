using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Module.Security.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Security.Domain.CQRS.Usuarios.Command
{
    public class ActivaUsuarioCommand : IRequest<Response<User>>, ITracking
    {
        public Guid TrackingId { get; set; }

        public String usuario { get; set; }

        public String password { get; set; }

        public string token { get; set; } //CODIGO POR USUARIO

        public string codigo { get; set; } //HASH GENERADO

        public bool requestToken { get; set; }

        public int? EscId { get; set; }
    }
}

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
    public class GetUserNoDocentePorDocumentacion : IRequest<Response<User>>, ITracking
    {
        public Guid TrackingId
        { get; set; }
        public string UsuNumeroDocumento
        { get; set; }
        public int TipDocId
        { get; set; }
        public bool isEdit { get; set; }
        public int EscId { get; set; }
    }
}

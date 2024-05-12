using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Module.Security.Domain.Entities;

namespace Module.Security.Domain.CQRS.Usuarios.Command
{
    public class RegistrarUsuarioCommand : IRequest<Response<User>>, ITracking
    {
        public Guid TrackingId { get; set; }

        public int UsuId { get; set; }
        public int TipDocId { get; set; }
        public string? UsuNumeroDocumento { get; set; }
        public DateTime? UsuFechaExpedicion { get; set; }
        public string? UsuPrimerNombre { get; set; }
        public string? UsuSegundoNombre { get; set; }
        public string? UsuPrimerApellido { get; set; }
        public string? UsuSegundoApellido { get; set; }
        public int GenId { get; set; }
        public int EstCivId { get; set; }
        public DateTime? UsuFechaNacimiento { get; set; }
        public int CiuId { get; set; }
        public decimal? UsuTelefonoContacto { get; set; }
        public string? UsuCorreoElectronico { get; set; }
        public string? UsuCorreoInstitucional { get; set; }
        public string? UsuDireccionInstitucional { get; set; }
        public string? UsuUsuario { get; set; }
        public string? UsuClave { get; set; }
        public bool UsuCambioClave { get; set; }
        public DateTime? UsuFechacambioclave { get; set; }
        public int? EstrId { get; set; }
        public bool? UsuActivo { get; set; }
        public int? SedId { get; set; }
        public int? PaisId { get; set; }
        public int? DepId { get; set; }
        public List<int>? RolId { get; set; }
        public List<int>? EscId { get; set; }
        public string? UsuAvatar { get; set; }


    }
}

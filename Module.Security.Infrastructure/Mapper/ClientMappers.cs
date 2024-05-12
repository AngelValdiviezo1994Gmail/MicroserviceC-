using Common.Domain.Wrappers;
//using Module.Security.Domain.CQRS.Command;
using Module.Security.Domain.CQRS.Usuarios.Command;
using Module.Security.Domain.Entities;
using Module.Security.Infrastructure.DTO;

namespace Module.Security.Infrastructure.Mapper
{
    public class ClientMappers : AutoMapper.Profile
    {
        public ClientMappers()
        {
            CreateMap<ResponsePaged<List<User>>, ResponsePaged<List<UserDTO>>>().ReverseMap();

            CreateMap<Response<User>, Response<UserDTO>>()
                .ReverseMap();


            CreateMap<User, UserDTO>()
                .ForMember(d => d.ChangePwd, opt => opt.MapFrom(s => s.ChangePwd))                
                .ReverseMap();

            CreateMap<Genero, GeneroDTO>().ReverseMap();


            CreateMap<Request<UserDTO>, RegistrarUsuarioCommand>()
                .ForMember(d => d.TipDocId, opt => opt.MapFrom(s => s.Data.TipDocId))
                .ForMember(d => d.UsuId, opt => opt.MapFrom(s => s.Data.UsuId))
                .ForMember(d => d.TipDocId, opt => opt.MapFrom(s => s.Data.TipDocId))
                .ForMember(d => d.UsuNumeroDocumento, opt => opt.MapFrom(s => s.Data.UsuNumeroDocumento))
                .ForMember(d => d.UsuFechaExpedicion, opt => opt.MapFrom(s => s.Data.UsuFechaExpedicion))
                .ForMember(d => d.UsuPrimerNombre, opt => opt.MapFrom(s => s.Data.UsuPrimerNombre))
                .ForMember(d => d.UsuSegundoNombre, opt => opt.MapFrom(s => s.Data.UsuSegundoNombre))
                .ForMember(d => d.UsuPrimerApellido, opt => opt.MapFrom(s => s.Data.UsuPrimerApellido))
                .ForMember(d => d.UsuSegundoApellido, opt => opt.MapFrom(s => s.Data.UsuSegundoApellido))
                .ForMember(d => d.GenId, opt => opt.MapFrom(s => s.Data.GenId))
                .ForMember(d => d.EstCivId, opt => opt.MapFrom(s => s.Data.EstCivId))
                .ForMember(d => d.UsuFechaNacimiento, opt => opt.MapFrom(s => s.Data.UsuFechaNacimiento))
                .ForMember(d => d.CiuId, opt => opt.MapFrom(s => s.Data.CiuId))
                .ForMember(d => d.UsuTelefonoContacto, opt => opt.MapFrom(s => s.Data.UsuTelefonoContacto))
                .ForMember(d => d.UsuCorreoElectronico, opt => opt.MapFrom(s => s.Data.UsuCorreoElectronico))
                .ForMember(d => d.UsuCorreoInstitucional, opt => opt.MapFrom(s => s.Data.UsuCorreoInstitucional))
                .ForMember(d => d.UsuDireccionInstitucional, opt => opt.MapFrom(s => s.Data.UsuDireccionInstitucional))
                .ForMember(d => d.UsuUsuario, opt => opt.MapFrom(s => s.Data.UsuUsuario))
                .ForMember(d => d.UsuClave, opt => opt.MapFrom(s => s.Data.UsuClave))
                .ForMember(d => d.UsuCambioClave, opt => opt.MapFrom(s => s.Data.UsuCambioClave))
                .ForMember(d => d.UsuFechacambioclave, opt => opt.MapFrom(s => s.Data.UsuFechacambioclave))
                .ForMember(d => d.EstrId, opt => opt.MapFrom(s => s.Data.EstrId))
                .ForMember(d => d.PaisId, opt => opt.MapFrom(s => s.Data.PaisId))
                .ForMember(d => d.DepId, opt => opt.MapFrom(s => s.Data.DepId))
                .ForMember(d => d.RolId, opt => opt.MapFrom(s => s.Data.RolId))
                .ForMember(d => d.EscId, opt => opt.MapFrom(s => s.Data.EscId))
                .ForMember(d => d.UsuActivo, opt => opt.MapFrom(s => s.Data.UsuActivo))
                .ForMember(d => d.SedId, opt => opt.MapFrom(s => s.Data.SedId))
                .ForMember(d => d.UsuAvatar, opt => opt.MapFrom(s => s.Data.UsuAvatar))
                .ReverseMap();

            CreateMap<Request<UserDTO>, ModificarUsuarioCommand>()
               .ForMember(d => d.TipDocId, opt => opt.MapFrom(s => s.Data.TipDocId))
               .ForMember(d => d.UsuId, opt => opt.MapFrom(s => s.Data.UsuId))
               .ForMember(d => d.TipDocId, opt => opt.MapFrom(s => s.Data.TipDocId))
               .ForMember(d => d.UsuNumeroDocumento, opt => opt.MapFrom(s => s.Data.UsuNumeroDocumento))
               .ForMember(d => d.UsuFechaExpedicion, opt => opt.MapFrom(s => s.Data.UsuFechaExpedicion))
               .ForMember(d => d.UsuPrimerNombre, opt => opt.MapFrom(s => s.Data.UsuPrimerNombre))
               .ForMember(d => d.UsuSegundoNombre, opt => opt.MapFrom(s => s.Data.UsuSegundoNombre))
               .ForMember(d => d.UsuPrimerApellido, opt => opt.MapFrom(s => s.Data.UsuPrimerApellido))
               .ForMember(d => d.UsuSegundoApellido, opt => opt.MapFrom(s => s.Data.UsuSegundoApellido))
               .ForMember(d => d.GenId, opt => opt.MapFrom(s => s.Data.GenId))
               .ForMember(d => d.EstCivId, opt => opt.MapFrom(s => s.Data.EstCivId))
               .ForMember(d => d.UsuFechaNacimiento, opt => opt.MapFrom(s => s.Data.UsuFechaNacimiento))
               .ForMember(d => d.CiuId, opt => opt.MapFrom(s => s.Data.CiuId))
               .ForMember(d => d.UsuTelefonoContacto, opt => opt.MapFrom(s => s.Data.UsuTelefonoContacto))
               .ForMember(d => d.UsuCorreoElectronico, opt => opt.MapFrom(s => s.Data.UsuCorreoElectronico))
               .ForMember(d => d.UsuCorreoInstitucional, opt => opt.MapFrom(s => s.Data.UsuCorreoInstitucional))
               .ForMember(d => d.UsuDireccionInstitucional, opt => opt.MapFrom(s => s.Data.UsuDireccionInstitucional))
               .ForMember(d => d.UsuUsuario, opt => opt.MapFrom(s => s.Data.UsuUsuario))
               .ForMember(d => d.UsuClave, opt => opt.MapFrom(s => s.Data.UsuClave))
               .ForMember(d => d.UsuCambioClave, opt => opt.MapFrom(s => s.Data.UsuCambioClave))
               .ForMember(d => d.UsuFechacambioclave, opt => opt.MapFrom(s => s.Data.UsuFechacambioclave))
               .ForMember(d => d.EstrId, opt => opt.MapFrom(s => s.Data.EstrId))
               .ForMember(d => d.PaisId, opt => opt.MapFrom(s => s.Data.PaisId))
               .ForMember(d => d.DepId, opt => opt.MapFrom(s => s.Data.DepId))
               .ForMember(d => d.RolId, opt => opt.MapFrom(s => s.Data.RolId))
               .ForMember(d => d.EscId, opt => opt.MapFrom(s => s.Data.EscId))
               .ForMember(d => d.UsuActivo, opt => opt.MapFrom(s => s.Data.UsuActivo))
               .ForMember(d => d.SedId, opt => opt.MapFrom(s => s.Data.SedId))
               .ForMember(d => d.UsuAvatar, opt => opt.MapFrom(s => s.Data.UsuAvatar))
               .ReverseMap();

        }
    }
}

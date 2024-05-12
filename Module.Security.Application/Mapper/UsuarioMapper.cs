using Module.Security.Domain.CQRS.Usuarios.Command;
using Module.Security.Domain.Entities;

namespace Module.Security.Application.Mapper
{
    public class UsuarioMapper : AutoMapper.Profile
    {
        public UsuarioMapper()
        {
            CreateMap<RegistrarUsuarioCommand, User>().ReverseMap();
        }
    }
}

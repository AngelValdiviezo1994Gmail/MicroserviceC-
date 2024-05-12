using Common.Domain.Wrappers;
using Module.Cliente.Domain.CQRS.Command;
using Module.Cliente.Domain.Entities;
//using Module.Cliente.Infrastructure.DTO;
using Comun = Common.Domain.Entities.Persistence;

namespace Module.Cliente.Infrastructure.Mapper
{
    public class ClientMappers : AutoMapper.Profile
    {
        public ClientMappers()
        {
            #region GÉNERO
            CreateMap<Genero, RegistrarGeneroCommand>().ReverseMap();                                             
            CreateMap<Genero, ModificarGeneroCommand>().ReverseMap();
            #endregion
        }
    }
}

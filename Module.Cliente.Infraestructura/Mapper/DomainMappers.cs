using Module.Security.Domain.Entities;
using Dominio = Module.Cliente.Domain.Entities;
using Persistencia = Common.Domain.Entities.Persistence;

namespace Module.Cliente.Infrastructure.Mapper
{
    public class DomainMappers : AutoMapper.Profile
    {
        public DomainMappers()
        {
            #region Entidades
            
            CreateMap<Dominio.Genero, Persistencia.Genero>().ReverseMap();

            #endregion
        }
    }
}

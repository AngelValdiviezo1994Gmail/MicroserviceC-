using Common.Domain.Wrappers;
using Module.Cliente.Domain.Entities;

namespace Module.Cliente.Infrastructura.Interfaces.Client
{
    public interface IGeneroClient
    {
        Task<ResponsePaged<List<Genero>>> GetGeneros(RequestPaged request);        
        Task<Response<Genero>> Registrar(Request<Genero> request);
        Task<Response<Genero>> Modificar(Request<Genero> request);
        Task<Response<bool>> Inactivar(Request<int> request);
        
    }
}
